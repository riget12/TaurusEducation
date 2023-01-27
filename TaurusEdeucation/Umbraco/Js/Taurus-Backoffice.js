function getLectors() {
  alert('a');
  $.ajax({
    url: '/Umbraco/Api/LectorMaintenance/GetLectors',
    type: 'GET',
    cache: false,
    dataType: "json",
    contentType: "application/json",
    success: alert('f')
  });
}

function banLector() {
  var email = document.getElementById("ban-lector").value;
  url = '/Umbraco/Api/LectorMaintenance/BanLector?email=' + email;

  $.ajax({
    url: url,
    type: 'POST',
    cache: false
  })
}

function unbanLector() {
  var email = document.getElementById("unban-lector").value;
  url = '/Umbraco/Api/LectorMaintenance/UnbanLector?email=' + email;

  $.ajax({
    url: url,
    type: 'POST',
    cache: false
  })
}

function deleteLector() {
  var email = document.getElementById("delete-lector").value;
  url = '/Umbraco/Api/LectorMaintenance/DeleteLector?email=' + email;

  $.ajax({
    url: url,
    type: 'POST',
    cache: false
  })
}