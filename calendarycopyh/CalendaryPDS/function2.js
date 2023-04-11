
const form = document.querySelector('form');
form.addEventListener('submit', function(event) {
  // Previne o comportamento padrão do formulário
  event.preventDefault();


  const mensagem = document.querySelector('#mensagem');
  mensagem.innerHTML = 'Evento marcado com sucesso!';
});