var app = new Vue({
    el: '#app',
    data: {
        games: [],
        scores: [],
        email: "",
        name: "",
        password: "",
        newpassword: "",
        confirmpassword: "",
        emailForPass: "",
        modal: {
            tittle: "",
            message: ""
        },
        player: null
    },
    mounted() {
        this.getGames();
        if(getQueryVariable('modal') === 'confirmemail') {
            this.confirmEmail();   
        }
        else if (getQueryVariable('modal') === 'resetpassword') {
            $("#resetpassword").modal('show');
        }
    },
    methods: {
        joinGame(gId) {
            var gpId = null;
            axios.post('/api/games/' + gId + '/players')
                .then(response => {
                    gpId = response.data;
                    window.location.href = '/game.html?gp=' + gpId;
                })
                .catch(error => {
                    alert("error al unirse al juego");
                });
        },
        createGame() {
            var gpId = null;
            axios.post('/api/games')
                .then(response => {
                    gpId = response.data;
                    window.location.href = '/game.html?gp=' + gpId;
                })
                .catch(error => {
                    alert("erro al obtener los datos");
                });
        },
        returnGame(gpId) {
            window.location.href = '/game.html?gp=' + gpId;
        },
        getGames: function (){
            this.showLogin(false);
            axios.get('/api/games')
                .then(response => {
                    this.player = response.data.email;
                    this.games = response.data.games;
                    this.getScores(this.games)
                    if (this.player == "Guest")
                        this.showLogin(true);
                })
                .catch(error => {
                    alert("error al obtener los datos");
                });
        },
        showModal: function (show) {
            if (show)
                $("#infoModal").modal('show');
            else
                $("#infoModal").modal('hide');
        },
        showLogin: function (show) {
            if (show) {
                $("#login-form").show();
                $("#login-form").trigger("reset");
                this.email = "";
                this.password = "";
            }
            else
                $("#login-form").hide();
        },
        logout: function () {
            axios.post('/api/auth/logout')
                .then(result => {
                    if (result.status == 200) {
                        this.showLogin(true);
                        this.getGames();
                    }
                })
                .catch(error => {
                    alert("Ocurrió un error al cerrar sesión");
                });
        },
        login: function(event){
            axios.post('/api/auth/login', {
                email: this.email, password: this.password
            })
                .then(result => {
                    if (result.status == 200) {
                        this.showLogin(false);
                        this.getGames();
                    }
                })
                .catch(error => {
                    console.log("error, código de estatus: " + error.response.status);
                    if (error.response.status == 401) {
                        this.modal.tittle = "Falló la autenticación";
                        //this.modal.message = "Email o contraseña inválido"
                        this.modal.message = error.response.data.message;
                        this.showModal(true);
                    }
                    else {
                        this.modal.tittle = "Fall&Oacute;la autenticaci&oacute;n";
                        this.modal.message = "Ha ocurrido un error";
                        this.showModal(true);
                    }
                });
        },
        signin: function (event) {
            axios.post('/api/players', {
                email: this.email, name: this.name, password: this.password
            })
                .then(result => {
                    if (result.status == 201) {
                        this.modal.tittle = "Registro Existoso";
                        this.modal.message = "Debes validar tu email";
                        this.showModal(true);
                        this.login();
                    }
                })
                .catch(error => {
                    console.log("error, código de estatus: " + error.response.status);
                    if (error.response.status == 403) {
                        this.modal.tittle = "Falló el registro";
                        this.modal.message = error.response.data
                        this.showModal(true);
                    }
                    else {
                        this.modal.tittle = "Fall&Oacute;la autenticaci&oacute;n";
                        this.modal.message = "Ha ocurrido un error";
                        this.showModal(true);
                    }
                });
        },
        forgetPassword: function (event) {
            axios.get('/api/auth/forgetpassword' + '?email=' + this.emailForPass)
                .then(result => {
                    if (result.status == 200) {
                        $("#forgetpassword").modal("hide");
                        this.modal.tittle = "Recuperar contraseña";
                        this.modal.message = "Dirígase a su dirección de correo electrónico";
                        this.showModal(true);
                    }
                })
                .catch(error => {
                    console.log("error, código de estatus: " + error.response.status);
                    if (error.response.status == 401) {
                        this.modal.tittle = "Recuperar contraseña";
                        this.modal.message = error.response.data.message;
                        this.showModal(true);
                    }
                    else {
                        this.modal.tittle = "Recuperar contraseña";
                        this.modal.message = "Ha ocurrido un error. Inténtelo de nuevo mas tarde";
                        this.showModal(true);
                    }
                });
        },
        resetPassword: function (event) {
            var email = getQueryVariable('email');
            var token = getQueryVariable('token');
            axios.post('/api/auth/resetpassword', {
                token: token, email: email, newpassword: this.newpassword, confirmpassword: this.confirmpassword
            })
                .then(result => {
                    if (result.status == 200) {
                        $("#resetpassword").modal("hide");
                        this.modal.tittle = "Recuperar contraseña";
                        this.modal.message = "La contraseña se ha actualizado correctamente. Inicie sesion.";
                        this.showModal(true);
                    }
                })
                .catch(error => {
                    console.log("error, código de estatus: " + error.response.status);
                    if (error.response.status == 400) {
                        this.modal.tittle = "Recuperar contraseña";
                        this.modal.message = error.response.data.message;
                        this.showModal(true);
                    }
                    else {
                        this.modal.tittle = "Recuperar contraseña";
                        this.modal.message = "Ha ocurrido un error. No se pudo actualizar la contraseña";
                        this.showModal(true);
                    }
                });
        },
        confirmEmail: function () {
            axios.get('/api/auth/confirmemail' + "?userid=" + getQueryVariable('userid') + "&token=" + getQueryVariable('token'))
                .then(response => {
                    this.modal.tittle = "Confirmar email";
                    this.modal.message = "Tu email ha sido confirmado";
                    this.showModal(true);
                })
                .catch(error => {
                    console.log("error, código de estatus: " + error.response.status);
                    if (error.response.status == 401) {
                        this.modal.tittle = "Confirmar email";
                        this.modal.message = error.response.data.message;
                        this.showModal(true);
                    }
                    else {
                        this.modal.tittle = "Confirmar email";
                        this.modal.message = "Ha ocurrido un error. No se pudo confirmar el mail, inténtelo de nuevo mas tarde.";
                        this.showModal(true);
                    }
                    
                });
        },
        getScores: function (games) {
            var scores = [];
            games.forEach(game => {
                game.gamePlayers.forEach(gp => {
                    var index = scores.findIndex(sc => sc.email == gp.player.email)
                    if (index < 0) {
                        var score = { email: gp.player.email, win: 0, tie: 0, lost: 0, total: 0 }
                        switch (gp.point) {
                            case 1:
                                score.win++;
                                break;
                            case 0:
                                score.lost++;
                                break;
                            case 0.5:
                                score.tie++;
                                break;
                        }
                        score.total += gp.point;
                        scores.push(score);
                    }
                    else {
                        switch (gp.point) {
                            case 1:
                                scores[index].win++;
                                break;
                            case 0:
                                scores[index].lost++;
                                break;
                            case 0.5:
                                scores[index].tie++;
                                break;
                        }
                        scores[index].total += gp.point;
                    }
                })
            })
            app.scores = scores;
        }
    },
    filters: {
        dateFormat(date) {
            return moment(date).format('LLL');
        }
    }
})

function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (let i = 0; i < vars.length; i++) {
        let pair = vars[i].split("=");
        if (pair[0].toUpperCase() == variable.toUpperCase()) {
            return pair[1];
        }
    }
    return null;
}