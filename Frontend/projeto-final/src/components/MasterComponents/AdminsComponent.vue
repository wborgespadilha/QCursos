<script setup>
import api from "../../services/api";
</script>

<template>
  <main>
    <h1 class="title">Administradores</h1>
    <hr />

    <!-- Modal add admin -->
    <div
      class="modal fade"
      id="addAdmin"
      data-bs-backdrop="static"
      data-bs-keyboard="false"
      tabindex="-1"
      aria-labelledby="staticBackdropLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content bg-dark">
          <div class="modal-header bg-dark">
            <h5 class="modal-title" id="staticBackdropLabel">
              Adicionar novo administrador
            </h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="CPF"
                  aria-label="CPF"
                  aria-describedby="basic-addon1"
                  v-model="newAdmin.cpf"
                />
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔒</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="Senha"
                  aria-label="Senha"
                  aria-describedby="basic-addon1"
                  style="color: #000"
                  v-model="newAdmin.password"
                />
              </div>

              <select
                class="form-select transparent"
                v-model="newAdmin.fk_institution"
              >
                <option
                  v-for="institution in allInstituition"
                  :value="institution.id"
                >
                  {{ institution.name }}
                </option>
              </select>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="registerAdmin()"
            >
              Confirmar
            </button>
            <button
              type="button"
              class="btn btn-outline-danger"
              data-bs-dismiss="modal"
            >
              Fechar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal edit admin -->
    <div
      class="modal fade"
      id="alterAdmin"
      data-bs-backdrop="static"
      data-bs-keyboard="false"
      tabindex="-1"
      aria-labelledby="staticBackdropLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content bg-dark">
          <div class="modal-header bg-dark">
            <h5 class="modal-title" id="staticBackdropLabel">
              Editar administrador
            </h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="CPF"
                  aria-label="CPF"
                  aria-describedby="basic-addon1"
                  v-model="newerAdmin.cpf"
                />
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔒</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="Senha"
                  aria-label="Senha"
                  aria-describedby="basic-addon1"
                  style="color: #000"
                  v-model="newerAdmin.password"
                />
              </div>

              <select
                class="form-select transparent"
                v-model="newerAdmin.fk_institution"
              >
                <option
                  v-for="institution in allInstituition"
                  :value="institution.id"
                >
                  {{ institution.name }}
                </option>
              </select>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="alterAdmin()"
            >
              Confirmar
            </button>
            <button
              type="button"
              class="btn btn-outline-danger"
              data-bs-dismiss="modal"
              v-on:click="selectAdmin(-1)"
            >
              Fechar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Button trigger add modal -->
    <div style="margin: 25px 0">
      <button
        class="btn btn-outline-primary"
        data-bs-toggle="modal"
        data-bs-target="#addAdmin"
      >
        Adicionar administrador da instituição
      </button>
    </div>

    <!-- Table of admins -->
    <div class="overflow">
      <table class="table table-striped">
        <thead>
          <tr>
            <th>Instituição</th>
            <th>Registro</th>
            <th>CPF</th>
            <th>Senha</th>
            <th colspan="2">Ações</th>
          </tr>
        </thead>
        <tbody v-for="admin in allAdmin">
          <tr>
            <td>{{ admin.institution_name }}</td>
            <td>{{ admin.registry }}</td>
            <td>{{ admin.cpf }}</td>
            <td class="hidePassword">{{ admin.password }}</td>
            <td>
              <button
                class="btn btn-outline-warning"
                data-bs-toggle="modal"
                data-bs-target="#alterAdmin"
                v-on:click="selectAdmin(admin)"
              >
                Editar
              </button>
            </td>
            <td>
              <button
                class="btn btn-outline-danger"
                v-on:click="deleteAdmin(admin.id)"
              >
                Excluir
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </main>
</template>

<style scoped>
.overflow {
  overflow-x: auto;
}
main {
  /* border: 1px solid red; */
  margin-top: 25px;
}
.form-control {
  color: #000 !important;
}
.title {
  font-weight: 600;
  color: #fff;
}
option {
  color: #000 !important;
}
.transparent {
  color: #fff !important;
  background-color: transparent;
}
hr {
  height: 1px;
  background-color: #fff;
  border: none;
}
.hidePassword {
  color: transparent !important;
  text-shadow: 0px 0px 10px #fff;
  transition: 0.2s;
}
.hidePassword:hover {
  color: #fff !important;
  text-shadow: none;
  transition: 0.2s;
}
</style>

<script>
export default {
  name: "AdminsComponent",
  async beforeMount() {
    await this.getAllAdmins();
    await this.getAllInstituitions();
  },
  data() {
    return {
      allAdmin: [],
      allInstituition: [],
      newAdmin: {
        cpf: "",
        password: "",
        fk_institution: 1,
      },
      adminCPFFind: "",
      newerAdmin: {
        cpf: "",
        password: "",
        fk_institution: 1,
      },
      currentAdmin: -1,
    };
  },
  methods: {
    async selectAdmin(admin) {
      this.currentAdmin = admin.id;
      this.newerAdmin.cpf = admin.cpf;
      this.newerAdmin.password = admin.password;
      this.newerAdmin.fk_institution = admin.fk_institution;
    },
    async alterAdmin() {
      if (
        /[A-Za-z!@#$ %^&*()_+\-=\[\]{};':"\\|,.<>\/?] +/.test(
          this.newerAdmin.cpf
        )
      ) {
        this.newerAdmin.cpf = 0;
        this.newerAdmin.password = "";
        this.newerAdmin.fk_institution = 1;

        return;
      }

      await api.put("/InstitutionAdmins", {
        id: this.currentAdmin,
        cpf: this.newerAdmin.cpf,
        password: this.newerAdmin.password,
        fk_institution: this.newerAdmin.fk_institution,
      });

      await this.getAllAdmins();
      this.newerAdmin.cpf = "";
      this.newerAdmin.password = "";
      this.newerAdmin.fk_institution = 0;
    },
    async getAllAdmins() {
      this.allAdmin = (await api.get("/InstitutionAdmins")).data;
    },
    async getAllInstituitions() {
      this.allInstituition = (await api.get("/Institution")).data;
    },
    async deleteAdmin(id) {
      await api.delete(`/InstitutionAdmins/${id}`);
      await this.getAllAdmins();
    },
    async registerAdmin() {
      if (
        /[A-Za-z!@#$ %^&*()_+\-=\[\]{};':"\\|,.<>\/?] +/.test(this.newAdmin.cpf)
      ) {
        this.newAdmin.cpf = 0;
        this.newAdmin.password = "";
        this.newAdmin.fk_institution = 1;
        return;
      }

      await api.post("/InstitutionAdmins", this.newAdmin);
      await this.getAllAdmins();
    },
  },
};
</script>
