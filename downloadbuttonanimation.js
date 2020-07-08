var downloadbutton = document.getElementById("downloadbutton");

var initialColor = "#ff2b00";
var mouseOnColor = "green"; 

downloadbutton.onmouseover = function() {
    downloadbutton.style.backgroundColor = mouseOnColor;
}

downloadbutton.onmouseout = function() {
    downloadbutton.style.backgroundColor = initialColor;
}