document.querySelectorAll(".faq-item").forEach((item) => {
  item.addEventListener("click", () => {
    const answer = item.querySelector("p");
    answer.style.display = answer.style.display === "block" ? "none" : "block";
  });
});