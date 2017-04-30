var htmlPdf = require('html-pdf');

module.exports = (result, options, data) => { 
  htmlPdf.create(data.html, options).toStream(function (err, stream) {
      if(err) result(err);
      return stream.pipe(result.stream);
    }); 
}