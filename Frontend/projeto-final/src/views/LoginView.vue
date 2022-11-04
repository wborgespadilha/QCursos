<script setup>
import api from "../services/api";
import "animate.css";
</script>

<template>
  <main>
    <div
      class="container animate__animated animate__fadeIn"
      style="max-width: 500px"
    >
      <h1>Login</h1>
      <form autocomplete="off">
        <div id="liveAlertPlaceholder" v-html="errorMessage"></div>
        <div class="input-group mb-3">
          <span class="input-group-text" id="basic-addon1">👤</span>
          <input
            type="text"
            min="1"
            class="form-control bg-dark text-white"
            aria-describedby="basic-addon1"
            v-model="logginUser.registry"
            :placeholder="userNamePlaceHolder"
          />
        </div>
        <div class="input-group mb-3">
          <span class="input-group-text" id="basic-addon1">🔒</span>
          <input
            :type="currentType"
            class="form-control bg-dark text-white"
            aria-describedby="basic-addon1"
            v-model="logginUser.password"
            :placeholder="userpasswordPlaceHolder"
          />
          <button
            v-if="currentType == 'password'"
            class="btn btn-outline-light"
            type="button"
            v-on:click="showPassword()"
          >
            📘
          </button>
          <button
            v-if="currentType == 'text'"
            class="btn btn-outline-light"
            type="button"
            v-on:click="showPassword()"
          >
            📖
          </button>
        </div>

        <div class="flex-left">
          <button class="btn btn-success" type="button" v-on:click="login()">
            Entrar
          </button>
          <div class="flex-right" style="width: 100%">
            <div class="form-check">
              <input
                class="form-check-input"
                type="radio"
                name="flexRadioDefault"
                id="flexRadioAluno"
                v-model="loginType"
                value="student"
                v-on:change="setInputType()"
              />
              <label class="form-check-label" for="flexRadioDefault1">
                Aluno
              </label>
            </div>
            <div class="form-check">
              <input
                class="form-check-input"
                type="radio"
                name="flexRadioDefault"
                id="flexRadioFuncionario"
                checked
                v-model="loginType"
                value="employee"
                v-on:change="setInputType()"
              />
              <label class="form-check-label" for="flexRadioDefault2">
                Funcionário
              </label>
            </div>
          </div>
        </div>
      </form>
    </div>
  </main>
</template>

<style scoped>
main {
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: var(--dark-darker);
  min-height: 100vh;

  background-image: url("../../src/assets/background.jpeg");
  background-blend-mode: darken;
}
.form-check {
  margin: 0 1rem;
}
.flex-left {
  display: flex;
  justify-content: left;
  align-items: center;
}
.flex-right {
  display: flex;
  justify-content: right;
  align-items: center;
}
</style>

<script>
export default {
  name: "LoginView",
  data() {
    return {
      logginUser: {
        registry: "",
        password: "",
      },
      userNamePlaceHolder: "Registro",
      userpasswordPlaceHolder: "Senha",
      loginType: "",
      currentType: "password",
      errorMessage: "",
    };
  },
  methods: {
    async showErrorMessage(message) {
      this.unShowErrorMessage();
      this.errorMessage = `<div class="alert alert-danger alert-dismissible" role="alert">
           <div class="text-black">${message}</div>
           <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
      </div>`;
    },
    async unShowErrorMessage() {
      this.errorMessage = "";
    },
    async setInputType() {
      if (this.loginType == "student") {
        this.currentType = "date";
        this.registryPlaceHolder = "Matrícula";
        this.userpasswordPlaceHolder = "Data de nascimento";
      } else {
        this.currentType = "text";
        this.registryPlaceHolder = "CPF";
        this.userpasswordPlaceHolder = "Senha";
      }
    },
    async clearLocal() {
      localStorage.clear();
    },
    async login() {
      if (
        this.logginUser.password.trim() == "" ||
        /[A-Za-z`!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/.test(
          this.logginUser.registry
        )
      ) {
        this.showErrorMessage(
          "Por favor, complete os campos corretamente para entrar no sistema"
        );
        this.logginUser.registry = "";
        this.logginUser.password = "";

        return;
      }

      if (this.loginType == "student") {
        // Estudante
        try {
          const user = {
            registry: this.logginUser.registry,
            born_date: this.logginUser.password,
          };

          const response = await api.post("/Authentication/loginStudent", user);

          if (response.data.length != 0) {
            localStorage.setItem(
              "@q-cursos:user",
              JSON.stringify(response.data[0])
            );
            this.$router.push("/c3R1ZGVudF9tZW51");
            return;
          } else {
            this.showErrorMessage("Usuários ou senha incorretos");
            return;
          }
        } catch (error) {
          console.error(error);
        }
      } else {
        let response;
        // Funcionário
        try {
          const user = {
            registry: String(this.logginUser.registry),
            password: String(this.logginUser.password),
          };

          // Master Admin
          response = await api.post("/Authentication/loginMADM", user);

          if (response.data.length != 0) {
            localStorage.setItem(
              "@q-cursos:user",
              JSON.stringify(response.data[0])
            );
            this.$router.push("/bWFzdGVyX21lbnU");
            return;
          }

          response = await api.post("/Authentication/loginIADM", user);

          if (response.data.length != 0) {
            localStorage.setItem(
              "@q-cursos:user",
              JSON.stringify(response.data[0])
            );
            this.$router.push("/YWRtaW5fbWVudQ");
            return;
          }

          response = await api.post("/Authentication/loginTeacher", user);

          if (response.data.length != 0) {
            localStorage.setItem(
              "@q-cursos:user",
              JSON.stringify(response.data[0])
            );
            this.$router.push("/dGVhY2hlcl9tZW51");
            return;
          }

          this.showErrorMessage("Usuários ou senha incorretos");

        } catch (error) {
          console.error(error);
        }
      }
      this.logginUser.registry = "";
      this.logginUser.password = "";
    },
    async showPassword() {
      this.currentType = this.currentType == "password" ? "text" : "password";
    },
    async setUserOnStorage(user, token) {
      localStorage.setItem("@q-cursos:user", JSON.stringify(user));
      localStorage.setItem("@q-cursos:token", token);
    },
  },
};
</script>
