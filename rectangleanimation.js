var canvas = document.getElementById("ballanimation");
var ctx = canvas.getContext("2d");



var x1 = 10;
var y1 = 10;
var x2 = 80;
var y2 = 20;

var x1_0 = 10;
var x2_0 = 80;

var dx = 1;

var color_list = ["black", "red", "blue", "green"]
var index = 0;

var animationIntervalId = -1;

function draw() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.beginPath();
    ctx.rect(x1, y1, x2, y2);
    ctx.fillStyle = "green";
    ctx.fill();
    ctx.closePath();

    if(x1 <= 100) {
        x1 += dx;
    }
    else {
        x1 = x1_0;
    }
}

canvas.onmouseover = function() {
    animationIntervalId = setInterval(draw, 10);
}

canvas.onmouseout = function() {
    clearInterval(animationIntervalId);
}