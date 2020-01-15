//var sliderLeft = document.getElementById("slider0to50");
//var sliderRight = document.getElementById("slider51to100");
//var inputMin = document.getElementById("min");
//var inputMax = document.getElementById("max");

//var inputMax = document.getElementById("input-number").max;
//var inputMin = document.getElementById("input-number").min;
var min = document.getElementById("myNumber2").min;
var max = document.getElementById("myNumber2").max;

//function sliderLeftInput() {//input udate slider left
//    sliderLeft.value = inputMin.value;
//}
//function sliderRightInput() {//input update slider right
//    sliderRight.value = (inputMax.value);//chnage in input max updated in slider right

//}

//calling function on change of inputs to update in slider
//inputMin.addEventListener("change", sliderLeftInput);
//inputMax.addEventListener("change", sliderRightInput);


/////value updation from slider to input
////functions to update from slider to inputs 
//function inputMinSliderLeft() {//slider update inputs
//    inputMin.value = sliderLeft.value;
//    //sliderPriceChange(inputMin.value);
//}
//function inputMaxSliderRight() {//slider update inputs
//    inputMax.value = sliderRight.value;
//    //sliderPriceChange(inputMax.value);
//}
//sliderLeft.addEventListener("change", inputMinSliderLeft);
//sliderRight.addEventListener("change", inputMaxSliderRight);
//sliderRight.addEventListener("change", inputMaxSliderRight);





function sliderPriceChange(inputMin,inputMax) {
    var url = window.location.href;
    if (url.endsWith("/")) {
        window.location.replace(url + "?query=Price%20%3A%20%5B" +inputMin+ "%20TO%20" +inputMax+ "%5D%26q=*%3A*%26rows=30%26start=0");
    }
    else {
        var array = url.split("q=", 1)
        if (url.includes("rows=30" && "TO") == true) {
            var res = url.replace(array[0], "?query=Price%20%3A%20%5B" + inputMin + "%20TO%20" + inputMax +"%5D%26");
        }
        else {
            var res = url.replace("?query=", "?query=Price%20%3A%20%5B" + inputMin + "%20TO%20" + inputMax +"%5D%26q=");
        }
        window.location.replace(res);
    }
}

function searchBySlider() {
    console.log(min)
    if (min == null || max == null) {
        sliderPriceChange(min, max)
    } else
    {
        sliderPriceChange(100, 200)
    }
    
}

