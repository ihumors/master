var app = {
    options: {
        headers: {
            "__RequestVerificationToken": $("#LoginForm input[name='__RequestVerificationToken']").val()
        },
        ajax: {
            login: { name: "", url: "" },
            verifyCode: { name: "", url: "" }
        }
    },
    init: function () {
        this.bindLoginSubmit();
    },
    bindLoginSubmit: function () {
        $("#LoginForm").submit(function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                url: '/Member/Security/Login',
                cache: false,
                headers: app.options.headers,
                data: $(this).serialize(),
                success: function (result) {
                    if (result.secucess) {
                        var returnUrl = app.request("returnUrl")
                        location.href = returnUrl == null ? "/" : returnUrl;
                        return;
                    }
                    alert(result.message);
                },
                error: function () {
                    alert("Error")
                }
            });
            return false;
        });
    },
    login: function () {

    },
    logOff: function () {

    },
    request: function (name) {
        var match = RegExp('[?&]' + name + '=([^&]*)')  
                        .exec(window.location.search);  
        return match && decodeURIComponent(match[1].replace(/\+/g, ' '));  

    }
}; 