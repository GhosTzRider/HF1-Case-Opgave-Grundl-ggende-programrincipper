// Callback function to load files
var loadFile = function (filePath, done) {
    var xhr = new XMLHTTPRequest();
    xhr.onload = function () { return done(this.responseText) }
    xhr.open("GET", filePath, true);
    xhr.send();
}




