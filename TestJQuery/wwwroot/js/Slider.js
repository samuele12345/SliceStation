const slider = document.querySelector(".slider");
const slides = document.querySelector(".slides");
const slide = document.querySelectorAll(".slide");

const imgObs = document.querySelectorAll(".img-obs");
const parObs = document.querySelectorAll(".par-obs");

// Parallax per le immagini dello slider
const sliderImages = document.querySelectorAll(".slide");

window.addEventListener('scroll', () => {
    const scrolled = window.pageYOffset;

    sliderImages.forEach(img => {
        // 0.5 = si muovono alla metà della velocità dello scroll normale (parallax)
        const translateY = scrolled * 0.5;

        img.style.transform = `translateY(${translateY}px)`;
    });
});


if (imgObs) {
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add("visible");

                observer.unobserve(entry.target);
            }

        })
    }, {
        threshold: 0.3,
    });

    imgObs.forEach(element => {
        observer.observe(element);
    });

    parObs.forEach(element => {
        observer.observe(element);
    })
}





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