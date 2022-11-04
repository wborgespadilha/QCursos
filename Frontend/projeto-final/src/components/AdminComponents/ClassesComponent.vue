<script setup>
import moment from "moment";
import api from "../../services/api";
</script>

<template>
  <main>
    <h1 class="title">Turmas</h1>
    <hr />

    <!-- Button trigger add modal -->
    <button
      type="button"
      class="btn btn-outline-primary"
      data-bs-toggle="modal"
      data-bs-target="#addClass"
    >
      Adicionar nova turma
    </button>

    <!-- Modal add class -->
    <div
      class="modal fade"
      id="addClass"
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
              Adicionar nova turma
            </h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="Nome"
                  aria-label="Nome"
                  aria-describedby="basic-addon1"
                  v-model="newClass.name"
                />
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">👨‍🏫</span>
                <select
                  class="form-select transparent"
                  v-model="newClass.fk_teacher"
                >
                  <option value="0" selected>Sem professor</option>
                  <option
                    v-for="teacher in allTeachers"
                    class="text-dark"
                    :value="teacher.id"
                  >
                    {{ teacher.name }}
                  </option>
                </select>
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <select
                  class="form-select transparent text-light"
                  v-model="newClass.fk_course"
                >
                  <option
                    v-for="course in allCourses"
                    class="text-dark"
                    :value="course.id"
                  >
                    {{ course.name }}
                  </option>
                </select>
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="registerClass()"
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

    <!-- Modal edit class -->
    <div
      class="modal fade"
      id="alterClass"
      data-bs-backdrop="static"
      data-bs-keyboard="false"
      tabindex="-1"
      aria-labelledby="staticBackdropLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content bg-dark">
          <div class="modal-header bg-dark">
            <h5 class="modal-title" id="staticBackdropLabel">Editar turma</h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="Nome"
                  aria-label="Nome"
                  aria-describedby="basic-addon1"
                  v-model="newerClass.name"
                />
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">👨‍🏫</span>
                <select
                  class="form-select transparent text-light"
                  v-model="newerClass.fk_teacher"
                >
                  <option class="text-dark" :value="undefined">
                    Sem professor
                  </option>
                  <option
                    v-for="teacher in allTeachers"
                    class="text-dark"
                    :selected="teacher.id == newerClass.fk_teacher"
                    :value="teacher.id"
                  >
                    {{ teacher.name }}
                  </option>
                </select>
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="alterClass()"
            >
              Confirmar
            </button>
            <button
              type="button"
              class="btn btn-outline-danger"
              data-bs-dismiss="modal"
              v-on:click="selectClass(-1)"
            >
              Fechar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Accordion with classes -->
    <div
      class="accordion accordion-flush bg-dark"
      id="accordionFlushExample"
      style="margin-top: 25px"
    >
      <div class="accordion-item bg-dark" v-for="classy in allClasses">
        <h2 class="accordion-header" id="flush-headingOne">
          <button
            class="accordion-button collapsed bg-dark"
            type="button"
            data-bs-toggle="collapse"
            :data-bs-target="
              '#class_' + classy.name.replaceAll(' ', '_').replaceAll('\'', '_')
            "
            aria-expanded="false"
            aria-controls="flush-collapseOne"
          >
            {{ allCourses.find((c) => c.id == classy.fk_course)?.name }} -
            {{ classy.name }}
          </button>
        </h2>
        <div
          :id="
            'class_' + classy.name.replaceAll(' ', '_').replaceAll('\'', '_')
          "
          class="accordion-collapse collapse"
          aria-labelledby="flush-headingOne"
          data-bs-parent="#accordionFlushExample"
        >
          <div class="accordion-body bg-dark">
            <div style="display: flex; justify-content: left; gap: 10px">
              <button
                class="btn btn-outline-warning"
                data-bs-toggle="modal"
                data-bs-target="#alterClass"
                v-on:click="selectClass(classy)"
              >
                Editar turma
              </button>
              <button
                class="btn btn-outline-danger"
                v-on:click="deleteClass(classy.id)"
              >
                Excluir turma
              </button>
            </div>
            <hr />
            <h4>Estudantes</h4>
            <h6>
              Professor:
              {{
                allTeachers.find((t) => t.id == classy.fk_teacher)?.name ??
                "Sem professor"
              }}
            </h6>
            <hr />
            <table class="table">
              <thead>
                <tr>
                  <th>Matrícula</th>
                  <th>Nome</th>
                  <th>Data de nascimento</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="student in classStudents">
                  <td v-if="student.fk_class == classy.id">
                    {{
                      allStudents.find((s) => s.id == student.fk_student)
                        ?.registry
                    }}
                  </td>
                  <td v-if="student.fk_class == classy.id">
                    {{
                      allStudents.find((s) => s.id == student.fk_student)?.name
                    }}
                  </td>
                  <td v-if="student.fk_class == classy.id">
                    {{ moment(allStudents.find((s) => s.id == student.fk_student)?.born_date).format("DD/MM/YYYY") }}
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
.left {
  display: flex;
  justify-content: left;
}
.right {
  display: flex;
  justify-content: right;
}
.title {
  font-weight: 600;
  color: #fff;
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
    await this.getAllTeachers();
    await this.getAllCourses();
    await this.getAllClasses();
    await this.getAllStudentByClass();
    await this.getAllStudents();
  },
  data() {
    return {
      currentLogged: {},
      allClasses: [],
      allCourses: [],
      allTeachers: [],
      classStudents: [],
      allStudents: [],

      newClass: {
        name: "",
        fk_teacher: 0,
      },
      newerClass: {
        name: "",
        fk_teacher: 0,
      },
      currentClass: -1,
      resultMessage: "",
    };
  },
  methods: {
    async showMessage() {
      const resultToast = document.getElementById("resultToast");
      const toast = new bootstrap.Toast(resultToast);
      toast.show();
    },
    async getAllStudentByClass() {
      const response = await api.get(
        `/StudentsClasses/Institution?Institution=${this.currentLogged.fk_institution}`
      );

      this.classStudents = response.data;
    },
    async getAllStudents() {
      const response = await api.get(
        `/Students/Institution?Institution=${this.currentLogged.fk_institution}`
      );

      this.allStudents = response.data;
    },
    async getAllClasses() {
      const response = await api.get(
        `/Classes/Institution?Institution=${this.currentLogged.fk_institution}`
      );

      this.allClasses = response.data;
    },
    async getAllCourses() {
      const response = await api.get(
        `/Courses/Institution?Institution=${this.currentLogged.fk_institution}`
      );

      this.allCourses = response.data;
    },
    async getAllTeachers() {
      const response = await api.get(
        `/Teachers/Institution?Institution=${this.currentLogged.fk_institution}`
      );

      this.allTeachers = response.data;
    },
    async registerClass() {
      this.newClass.name = this.newClass.name.replaceAll('.', '_');
      await api.post("/Classes", this.newClass);

      await this.getAllClasses();
      await this.getAllTeachers();
      await this.getAllCourses();
    },
    async selectClass(classy) {
      this.currentClass = await classy.id;
      this.newerClass.name = await classy.name;
      this.newerClass.fk_teacher = await classy.fk_teacher;
    },
    async alterClass() {
      const response = await api.put("/Classes", {
        id: this.currentClass,
        name: this.newerClass.name.replaceAll('.', '_'),
        fk_teacher: this.newerClass.fk_teacher,
      });

      await this.getAllClasses();
      await this.getAllCourses();

      this.currentCourse = -1;
    },
    async deleteClass(id) {
      await api.delete(`/Classes/${id}`).catch(async (err) => {
        this.resultMessage = await err.response.data;
        await this.showMessage();
      });

      this.getAllClasses();
    },
    async addStudentToClass() {},
    async removeStudentFromClass() {},
  },
};
</script>
