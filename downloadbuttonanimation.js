var downloadbutton = document.getElementById("downloadbutton");

var initialColor = "red";
var mouseOnColor = "green"; 

downloadbutton.onmouseover = function() {
    downloadbutton.style.backgroundColor = mouseOnColor;
}

downloadbutton.onmouseout = function() {
    downloadbutton.style.backgroundColor = initialColor;
}