(function () {
  function ready(fn) {
    if (document.readyState !== "loading") fn();
    else document.addEventListener("DOMContentLoaded", fn);
  }

  ready(function () {
    var toggle = document.querySelector(".nav-toggle");
    var nav = document.querySelector(".nav-bar");
    if (toggle && nav) {
      toggle.addEventListener("click", function () {
        var open = nav.classList.toggle("is-open");
        toggle.setAttribute("aria-expanded", open ? "true" : "false");
        toggle.setAttribute("aria-label", open ? "Close menu" : "Open menu");
      });
    }

    // Mark current nav link from pathname
    var path = (window.location.pathname || "").split("/").pop() || "Home.aspx";
    path = path.toLowerCase();
    document.querySelectorAll(".main-nav a").forEach(function (link) {
      var href = (link.getAttribute("href") || "").split("/").pop().toLowerCase();
      if (href && href === path) {
        link.classList.add("is-active");
        link.setAttribute("aria-current", "page");
      }
    });
  });
})();
