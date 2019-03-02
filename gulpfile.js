var gulp = require("gulp");
var fs = require("fs");
var runSequence = require("run-sequence");
var gulpNugetRestore = require("gulp-nuget-restore");
var gulpMsbuild = require("gulp-msbuild");
var gulpForeach = require("gulp-foreach");
var gulpDebug = require("gulp-debug");
var unicorn = require("./scripts/unicorn.js");
var habitat = require("./scripts/habitat.js");

var config;
if (fs.existsSync("./gulp-config.user.js")) {
    config = require("./gulp-config.user.js")();
}
else {
    config = require("./gulp-config.js")();
}

module.exports.config = config;

gulp.task("default", function(callback) {
	config.runCleanBuilds = true;
	
	return runSequence(
		"Nuget-Restore",
		"Publish-All-Projects",
		callback);
});



/*****************************
  Helpers
*****************************/


/*****************************
  Initial setup
*****************************/

gulp.task("Nuget-Restore", function(callback) {
	var solution = "./" + config.solutionName + ".sln";
	
	return gulp.src(solution)
		.pipe(gulpNugetRestore());
});

gulp.task("Publish-All-Projects", function(callback) {
	return runSequence(
		"Build-Solution",
		"Publish-Foundation-Projects",
		"Publish-Feature-Projects",
		"Publish-Project-Projects",
		"Apply-Xml-Transform",
		"Sync-Unicorn",
		"Publish-Transforms",
		callback);
});

gulp.task("Build-Solution", function(callback) {
	var targets = ["Build"];
	
	if (config.runCleanBuilds) {
		targets = ["Clean", "Build"];
	}
	
	var solution = "./" + config.solutionName + ".sln";
	return gulp.src(solution)
		.pipe(gulpMsbuild({
			targets: targets,
			configuration: config.buildConfiguration,
			logCommand: false,
			verbosity: config.buildVerbosity,
			stdout: true,
			errorOnFail: true,
			maxcpucount: config.buildMaxCpuCount,
			nodeReuse: false,
			toolsVersion: config.buildToolsVersion,
			properties: {
				Platform: config.buildPlatform,
				WarningLevel: config.warningLevel
			}
		}));
});

gulp.task("Sync-Unicorn", function(callback) {
	var options = {};
	options.instanceUrl = config.instanceUrl;
	options.siteHostName = habitat.getSiteUrl();
	options.authenticationConfigFile = config.websiteRoot + "/App_config/Include/DreamTeam.Unicorn.SharedSecret.config";
	options.maxBuffer = Infinity;
	
	unicorn(function () { return callback() }, options);
});

gulp.task("Publish-Transforms", function(callback) {
	return gulp.src("./src/**/code/**/*.xdt")
		.pipe(gulp.dest(config.websiteRoot + "/temp/transforms"));
});


/*****************************
  Publish
*****************************/
var publishProjects = function(location, dest) {
    dest = dest || config.websiteRoot;

    console.log("publish to " + dest + " folder");
    return gulp.src([location + "/**/code/*.csproj"])
        .pipe(gulpForeach(function (stream, file) {
            return publishStream(stream, dest);
        }));
};

var publishStream = function (stream, dest) {
	var targets = ["Build"];

	return stream
		.pipe(gulpDebug({ title: "Building project:" }))
		.pipe(gulpMsbuild({
			targets: targets,
			configuration: config.buildConfiguration,
			logCommand: false,
			verbosity: config.buildVerbosity,
			stdout: true,
			errorOnFail: true,
			maxcpucount: config.buildMaxCpuCount,
			nodeReuse: false,
			toolsVersion: config.buildToolsVersion,
			properties: {
				Platform: config.publishPlatform,
				DeployOnBuild: "true",
				DeployDefaultTarget: "WebPublish",
				WebPublishMethod: "FileSystem",
				BuildProjectReferences: "false",
				DeleteExistingFiles: "false",
				publishUrl: dest
			}
		}));
};

gulp.task("Publish-Feature-Projects", function(callback) {
	return publishProjects("./src/Feature");
});

gulp.task("Publish-Foundation-Projects", function(callback) {
	return publishProjects("./src/Foundation");
});

gulp.task("Publish-Project-Projects", function(callback) {
	return publishProjects("./src/Project");
});

gulp.task("Apply-Xml-Transform", function(callback) {
	var layerPathFilters = [
		"./src/Foundation/**/*.xdt", "./src/Feature/**/*.xdt", "./src/Project/**/*.xdt",
		"!./src/**/obj/**/*.xdt", "!./src/**/bin/**/*.xdt", "!./src/**/web.config.Debug.xdt", "!./src/**/web.config.Release.xdt"
	];
	
	return gulp
		.src(layerPathFilters)
		.pipe(gulpForeach(function (stream, file) {
			var fileToTransform = file.path
				.replace(/.+code\\(.+)\.xdt/, "$1")
				.replace("\.sc-internal", "");
				
			util.log("Applying configuration transform: " + file.path);
			
			return gulp.src("./scripts/applytransform.targets")
				.pipe(gulpMsbuild({
					targets: ["ApplyTransform"],
					configuration: config.buildConfiguration,
					logCommand: false,
					verbosity: config.buildVerbosity,
					stdout: true,
					errorOnFail: true,
					maxcpucount: config.buildMaxCpuCount,
					nodeReuse: false,
					toolsVersion: config.buildToolsVersion,
					properties: {
						Platform: config.buildPlatform,
						WebConfigToTransform: config.websiteRoot,
						TransformFile: file.path,
						FileToTransform: fileToTransform
					}
				}));
		}));
});