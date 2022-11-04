<script setup>
import moment from "moment";
import api from "../../services/api";
</script>

<template>
  <main>
    <h1 class="title">Suas turmas</h1>
    <hr />

    <!-- Accordion with classes -->
    <div class="accordion accordion-flush bg-dark" id="accordionFlushExample">
      <div class="accordion-item bg-dark" v-for="_class in allClasses">
        <h2 class="accordion-header bg-dark" id="flush-headingOne">
          <button
            class="accordion-button collapsed bg-dark"
            type="button"
            data-bs-toggle="collapse"
            :data-bs-target="
              '#class_' + _class.name.replaceAll(' ', '_').replaceAll('\'', '_')
            "
            aria-expanded="false"
            aria-controls="flush-collapseOne"
          >
            {{ _class.course_name }} - {{ _class.name }}
          </button>
        </h2>
        <div
          :id="
            'class_' + _class.name.replaceAll(' ', '_').replaceAll('\'', '_')
          "
          class="accordion-collapse collapse bg-dark"
          aria-labelledby="flush-headingOne"
          data-bs-parent="#accordionFlushExample"
        >
          <div class="accordion-body bg-dark">
            <button
              type="button"
              class="btn btn-outline-primary"
              v-on:click="calculateAverages(_class.id)"
              style="margin: 1rem 0"
            >
              Fechar médias
            </button>
            <table class="table">
              <thead>
                <th>Matrícula</th>
                <th>Nome</th>
                <th>Data de nascimento</th>
              </thead>
              <tbody>
                <tr v-for="studentClass in allStudentClasses">
                  <td v-if="studentClass.fk_class == _class.id">
                    {{
                      allStudents.find((s) => s.id == studentClass.fk_student)
                        .registry
                    }}
                  </td>
                  <td v-if="studentClass.fk_class == _class.id">
                    {{
                      allStudents.find((s) => s.id == studentClass.fk_student)
                        .name
                    }}
                  </td>
                  <td v-if="studentClass.fk_class == _class.id">
                    {{
                      moment(
                        allStudents.find((s) => s.id == studentClass.fk_student)
                          .born_date
                      ).format("DD/MM/YYYY")
                    }}
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>

    <!-- Toast -->
    <div class="toast-container position-fixed bottom-0 end-0 p-3">
      <div
        id="resultToast"
        class="toast bg-dark"
        role="alert"
        aria-live="assertive"
        aria-atomic="true"
      >
        <div class="toast-header bg-dark">
          <strong class="me-auto text-light">📡 Sistema</strong>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="toast"
            aria-label="Close"
          ></button>
        </div>
        <div class="toast-body bg-dark text-light">{{ resultMessage }}</div>
      </div>
    </div>
  </main>
</template>

<style scoped>
main {
  /* border: 1px solid red; */
  margin-top: 25px;
}
.title {
  font-weight: 600;
  color: #fff;
}
.td {
  /* border: 1px solid #fff; */
  padding: 10px 5px;
  font-weight: 600;
}
hr {
  height: 1px;
  background-color: #fff;
  border: none;
}
</style>

<script>
export default {
  name: "ClassesComponent",
  async beforeMount() {
    this.currentLogged = await JSON.parse(
      localStorage.getItem("@q-cursos:user")
    );
    await this.getAllClasses();
    await this.getAllStudents();
    await this.getAllStudentClasses();
  },
  data() {
    return {
      currentLogged: {},
      allClasses: [],
      allStudents: [],
      allStudentClasses: [],

      resultMessage: "",
    };
  },
  methods: {
    async showMessage() {
      const toast = new bootstrap.Toast(resultToast);
      toast.show();
    },
    async getAllClasses() {
      this.allClasses = (
        await api.get(`/Classes/Teacher?Teacher=${this.currentLogged.id}`)
      ).data;
    },
    async getAllStudents() {
      this.allStudents = (
        await api.get(
          `/Students/Institution?Institution=${this.currentLogged.fk_institution}`
        )
      ).data;
    },
    async getAllStudentClasses() {
      this.allStudentClasses = (
        await api.get(
          `/StudentsClasses/Institution?Institution=${this.currentLogged.fk_institution}`
        )
      ).data;
    },
    async calculateAverages(id) {
      const response = await api.get(`/GenerateAverages/${id}`);

      this.resultMessage = await response.data;
      await this.showMessage();
    },
  },
};
</script>
