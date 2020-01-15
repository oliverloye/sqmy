var sliderLeft = document.getElementById("slider0to50");
var sliderRight = document.getElementById("slider51to100");
var inputMin = document.getElementById("min");
var inputMax = document.getElementById("max");




function price_ascending() {
    var url = window.location.href;
    if (url.endsWith("/")) {
        window.location.replace(url + "?query=*%3A*%26rows=30%26sort=Price%20asc%26start=0");
    }
    else {
        if (url.includes("sort=Price%20desc")==true) {
            var res = url.replace("sort=Price%20desc", "sort=Price%20asc");
            console.log("1");
        }
        else if (url.includes("sort=Price%20asc") == true) {
            var res = url.replace("sort=Price%20asc", "sort=Price%20asc")
            console.log("2");
        }
        else {
            var res = url.replace("rows=30%26", "rows=30%26sort=Price%20asc%26");
            console.log("3");
            
        }
        
        window.location.replace(res);
    }
}

function price_descending() {
    var url = window.location.href;
    if (url.endsWith("/")) {
        window.location.replace(url + "?query=*%3A*%26rows=30%26sort=Price%20desc%26start=0");
    }
    else {
        if (url.includes("sort=Price%20asc") == true) {
            
            var res = url.replace("sort=Price%20asc", "sort=Price%20desc")       
            console.log("4");
        }
        else if (url.includes("sort=Price%20desc") == true) {
            var res = url.replace("sort=Price%20desc", "sort=Price%20desc");
            console.log("5");
        }
        else {
            var res = url.replace("rows=30%26", "rows=30%26sort=Price%20desc%26");
            console.log("6");
            
        } 
       
        window.location.replace(res);
    }
}
