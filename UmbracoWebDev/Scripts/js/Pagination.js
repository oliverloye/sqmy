function pagination_function_right() {
    var url = window.location.href;
    if (url.endsWith("/")) {
        window.location.replace(url+"?query=*%3A*%26rows=30%26start=30");
    }
    else {
        var arr = url.split('=')
        var number = arr.pop();
        var newnumber = parseInt(number) + 30;
        var res = url.replace("start=" + number,"start="+newnumber)
        window.location.replace(res)
    }
}

function pagination_function_left() {
    var url = window.location.href;
    if (url.endsWith("/")) {
        window.location.replace(url+"?query=*%3A*%26rows=30%26start=0");
    }
    else {
        var arr = url.split('=')
        var number = arr.pop();
        var newnumber = parseInt(number) - 30;
        if (newnumber < 0) {
            window.location.replace(url);
        }
        else {
            var res = url.replace("start=" + number, "start=" + newnumber)
            window.location.replace(res)
        }
        
    }
}

var currentPage = document.getElementById("PageOverview");
var numberOfResultes = parseInt(document.getElementById("numberOfResults").innerHTML);
var Pages = Math.ceil(numberOfResultes / 30);
var thisPage = 1;
var url = window.location.href;
if (url.endsWith("/")) {
    currentPage.innerHTML ="1 / " + Pages;
}
else {
    var arr = url.split('=')
    var number = arr.pop();
    thisPage = Math.floor(parseInt(number) / 30);
    if (thisPage == 0) {
        currentPage.innerHTML ="1 / " + Pages;
    }
    else if (thisPage == 1) {
        currentPage.innerHTML = "2 / " + Pages;
    }
    else{
        currentPage.innerHTML = (thisPage + 1) + " / " + Pages;
    }
}




