module.exports = function () {
	var prefix = "dream-team";
	
	var instancesRoot = "C:\\inetpub\\wwwroot\\";
	
	var sitecoreInstanceName = prefix + ".dev.local";
    var sitecoreInstanceRoot = instancesRoot + sitecoreInstanceName + "\\";
    var sitecoreInstanceUrl = "http://" + sitecoreInstanceName + "/";
	
	var xConnectInstanceName = sitecoreInstanceName + ".xconnect";
	var xConnectInstanceRoot = instancesRoot + xConnectInstanceName + "\\";
	
	var solutionName = "DreamTeam";
    
	var config = {
        websiteRoot: sitecoreInstanceRoot,
        instanceUrl: sitecoreInstanceUrl,
        xConnectRoot: xConnectInstanceRoot,
        sitecoreLibraries: sitecoreInstanceRoot + "\\bin",
        solutionName: solutionName,
        buildConfiguration: "Debug",
        buildToolsVersion: 15.0,
        buildMaxCpuCount: 0,
        buildVerbosity: "minimal",
        buildPlatform: "Any CPU",
        publishPlatform: "AnyCpu",
        runCleanBuilds: false,
        warningLevel: 4,
        messageStatisticsApiKey: "97CC4FC13A814081BF6961A3E2128C5B",
        marketingDefinitionsApiKey: "DF7D20E837254C6FBFA2B854C295CB61",
    };
	
    return config;
}
