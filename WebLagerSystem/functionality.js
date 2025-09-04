readTextFile("export/Hans Jensen_products.json", function (text) {
    var data = JSON.parse(text);
    console.log(data);

