function helloWorld() {
    console.log("Hello World");
}

const saudacao = () => {
    var data = new Date();
    return data.getHours() <= 12 ? "Bom dia!" : data.getHours() <= 18 ? "Boa tarde!" : "Boa Noite!";
}

helloWorld();
console.log("A saudacao do momento e " + saudacao());