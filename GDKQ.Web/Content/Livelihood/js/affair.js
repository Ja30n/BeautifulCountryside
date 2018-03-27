// JavaScript Document

window.onload = function(){
       setInterval(function() {
            runtime();
        },2800);

    var list = document.getElementsByClassName("outside");
    list[0].style.backgroundColor="#63b2f5";

}



function  listclicks(n){
    var list = document.getElementsByClassName("outside");
    for (var i = 0 ; i <list.length;i++){
        list[i].style.backgroundColor="#f5f5f5";
        console.log(list[i]);
    }
    console.log(n);
    console.log(list[n]);
    list[n].style.backgroundColor="#63b2f5";
}


var i = 0;
var arr = ["/Content/Livelihood/images/af_img1.png", "/Content/Livelihood/images/af_img2.png", "/Content/Livelihood/images/af_img3.png"];
var arrp = ["修葺一心斋", "恭贺我村入选第二批中国传统村落名录", "巩固旧屋"];
function  runtime() {
    var p = document.getElementById("p1");
    var img1 = document.getElementById("af_img");
    if (i == 3) {
        i = 0;
    }
    p.innerHTML=arrp[i];
    img1.src = arr[i];
    i = i+1;
}


function clicks(){
    var img2 = document.getElementById("af_img").src;
    var imgs = img2.substring(img2.lastIndexOf("/")+1);
    if (imgs==="1.jpg"){
        window.location.href="http://news.tzc.edu.cn/index.php/campus-updates/3180.html"
    }
    if (imgs==="2.jpg"){
        window.location.href="http://news.tzc.edu.cn/index.php/campus-updates/3658.html"
    }
    if (imgs==="3.jpg"){
        window.location.href="http://news.tzc.edu.cn/index.php/campus-updates/3312.html"
    }
}
