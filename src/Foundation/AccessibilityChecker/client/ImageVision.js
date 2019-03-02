(function(window) {
    /**
     * See how to use api on https://cloud.google.com/vision/docs
     * @param {{requests:Array<{
     *  image:{source:{imageUri: string}},
     *  features: Array
     * }>}} configs
     * @param {boolean} isMock
     * @param {function} callback
     */
    var imageVision = function(configs, callback, isMock) {
        if (isMock) {
            // returns mock data
            var mockData = {"responses": [{"landmarkAnnotations": [{"mid": "/m/019dvv", "description": "Mount Rushmore", "score": 0.8757957, "boundingPoly": {"vertices": [{"x": 321, "y": 195}, {"x": 777, "y": 195}, {"x": 777, "y": 510}, {"x": 321, "y": 510}]}, "locations": [{"latLng": {"latitude": 43.878264, "longitude": -103.45700740814209}}]}], "webDetection": {"webEntities": [{"entityId": "/m/019dvv", "score": 255.8336, "description": "Mount Rushmore National Memorial"}, {"entityId": "/m/0373w4", "score": 12.14976, "description": "Crazy Horse Memorial"}], "fullMatchingImages": [{"url": "https://tinahanagan.files.wordpress.com/2012/04/dsc_0597.jpg"}], "partialMatchingImages": [{"url": "https://i1.wp.com/eightsails.files.wordpress.com/2016/10/img_2138.jpg"}, {"url": "https://i1.wp.com/littlezenmonkey.com/wp-content/uploads/2016/05/IMG_6994.jpg"}], "visuallySimilarImages": [{"url": "http://media.gettyimages.com/photos/the-famous-landmark-mount-rushmore-on-a-perfect-weather-day-picture-id106682515?s=170667a"}, {"url": "http://www.clevelandseniors.com/images/funny/mount-rushmore.jpg"}]}}]};
            callback(mockData);
            return;
        }

        var xhr = new XMLHttpRequest();

        // sitecore setups key for security reason
        xhr.open('GET', 'https://vision.googleapis.com/v1/images:annotate?key='+window.VISION_KEY, true);

        // send the request to api
        xhr.send();

        xhr.onreadystatechange = function() { // (3)
            if (xhr.readyState != 4) return;
            if (xhr.status != 200) {
                callback(xhr.responseText)
            }
        }
    };

    if (typeof window.getComputedStyle === 'function') {
        window.imageVision = imageVision;
    }
})(window);
