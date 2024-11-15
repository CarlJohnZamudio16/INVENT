document.addEventListener("DOMContentLoaded", function () {
    const button = document.querySelector('.btn-home-add');

    // Add the "alive" class to make the button pulse
    button.classList.add('alive');

    button.addEventListener('mousemove', function (e) {
        const buttonRect = button.getBoundingClientRect();
        const buttonWidth = buttonRect.width;
        const buttonHeight = buttonRect.height;

        // Calculate mouse position relative to the viewport
        const mouseX = e.clientX;
        const mouseY = e.clientY;

        // Randomly move the button to avoid the cursor
        const maxX = window.innerWidth - buttonWidth;
        const maxY = window.innerHeight - buttonHeight;

        const randomX = Math.random() * maxX;
        const randomY = Math.random() * maxY;

        // Make sure the button moves away from the cursor and avoids getting too close
        if (Math.abs(mouseX - buttonRect.left) < 100 && Math.abs(mouseY - buttonRect.top) < 100) {
            button.style.left = `${randomX}px`;
            button.style.top = `${randomY}px`;
        }
    });
});
