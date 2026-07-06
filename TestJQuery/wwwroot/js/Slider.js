const slider = document.querySelector(".slider");
const slides = document.querySelector(".slides");
const slide = document.querySelectorAll(".slide");



if (slider) {
    let slideIndex = 0;
    let intervalId = null;

    window.addEventListener("DOMContentLoaded", slideDefault)


    function slideDefault() {
        slide[slideIndex].classList.add("active");
        intervalId = setInterval(nextSlide, 5000);
    }

    function currentSlide(index) {
        if (index == slide.length) {
            index = 0;
        } else if (index == -1) {
            index = slide.length - 1;
        }

        slide.forEach(slide => {
            slide.classList.remove("active");
        })

        slideIndex = index;
        slide[slideIndex].classList.add("active");
    }


    function nextSlide() {
        slideIndex++;
        clearInterval(intervalId);
        intervalId = setInterval(nextSlide, 5000);
        currentSlide(slideIndex);
    }

}