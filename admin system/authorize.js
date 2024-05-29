window.onload = async function () {
    await LoadRole(null);
    await LoadUser();
}

async function LoadRole(name) {
    var url = "http://localhost:5188/api/SuperAdmin/roles?name=" + name
    var request = new Request(url);
    var rs = await fetch(request);
    if (rs.ok) {
        var txt = await rs.text();
        var pTxt = JSON.parse(txt);
        pTxt.value.forEach(element => {
            document.getElementById('roleTable').innerHTML += ` <tr>
            <td>${element.id}</td>
            <td>${element.name}</td>
        </tr>`
        });
    }
}

async function LoadUser() {
    var url = "http://localhost:5188/api/User/users"
    var request = new Request(url);
    var rs = await fetch(request);
    if (rs.ok) {
        var txt = await rs.text();
        var pTxt = JSON.parse(txt);
        pTxt.value.forEach(element => {
            document.getElementById('userTable').innerHTML += ` <tr>
            <td>${element.id}</td>
            <td>${element.username}</td>
            <td>${element.role.id}</td>
            <td>${element.role.name}</td>
        </tr>`
        });
    }
}