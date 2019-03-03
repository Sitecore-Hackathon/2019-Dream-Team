![Hackathon Logo](documentation/images/hackathon.png?raw=true "Hackathon Logo")

# Accessibility Checker Module

## Module Purpose

Any Market has depends upon validation of a Web Content accessibility. This is vital required for online business.
Accessibility Checker Module will allow to validate and evalution ways of improvements with designed Web content with different access levels.
This module will help Content Authors and Marketers ensure their Web content is properly structured and Media content accessability is validated in Experience Editor (XP).

## Module Sitecore Hackathon Category

Best enhancement to the Sitecore Admin (XP) UI for Content Editors & Marketers

## Prerequisites
Solution includes Sitecore Helix structured projects: DreamTeam.Foundation.AccessibilityChecker, DreamTeam.Project.Website.
Solution contains Unicorn serialization and ability to deploy on Sitecore 9.1 instance using *GULP*  task.
For proper Unicorn serialization, please, copy `DreamTeam.Project.Common.DevSettings.config` and check the `sourceFolder` variable path value (should be pointed on `src` folder of your cloned git source code).

## Installation Guide

 - clone repository to local machine
 - open `DreamTeam.sln` solution file in Visual Studion 2017
 - restore nuget packages for the solution
 - run *GULP* task `gulp default` to deploy solution into local Sitecore instance. By default, instance name is: `dream-team.dev.local`, you can easily modify instance name in `gulp-config.js` file modifing variable `sitecoreInstanceName`
 - alternative of Unicorn deployment, install DreamTeamModule-1.0.zip in local site instance
 - have a fun :)

## Module usage

### Web site
Accessibility Checker can be accessed by Content Editors from Experience Editor. Once, you rendered any page we will have ability to review accessibility of particular field (in our case, we used `Image` field type):
![Image Field Accessibility Check Button](documentation/images/Image Field Accessibility Check.png?raw=true "Image Field Accessibility Check Button")

accessibility of the whole rendering:
![Rendering Accessibility Check Button](documentation/images/Component Accessibility Check.png?raw=true "Image Field Accessibility Check Button")

and verify and suggest how to improve accessibility of the page:
![Page Accessibility Check Button](documentation/images/Page Level Accessibility Check.png?raw=true "Page Accessibility Check Button")

In testing purposes, we made a stubs in calling 3rd party services for accessibility validation due to `API KEY` and non-free services uses.

Our module includes an example of accessibility check for media content (`Image` field rendering). TO make it possible `alt` tag of the image should be filled out with proper image content description. We decide to use `Vision API` from `Google Cloud` toolkit. As well, it's possible to find similar service in `Microsoft` toolkit. `Vision API` allows to send media URL (*BE AWARE, your media item should be Published and be accessible out side*) like [https://cloud.google.com/vision/images/rushmore.jpg][image url].
As a response in `json` format we will get following:
![Vision API Json Response Format](documentation/images/VisionAPIJsonResponse.png?raw=true "Vision API Json Response Format")

Content Editors will have ability to review the best scores with media content description and fill out `alt` tag with corresponding text.

As well, for `Rendering` level and `Page` mode we should send generated `html` code, respectively. Review and analyzing html output 3rd party services (such like [AXE API][AXE API]) will provide proper advices and suggestions how to improve content accessibility. 

## Video
Unfortunately, we didnt provided recorded video :(
[image url]: https://cloud.google.com/vision/images/rushmore.jpg
[AXE API]: https://www.deque.com/axe/documentation/api-documentation/