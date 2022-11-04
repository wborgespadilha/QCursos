<template>
  <main class="main bg-darker">
    <!-- Navbar -->
    <nav class="navbar navbar-expand-lg navbar-dark transparentDark">
      <div class="container-fluid">
        <!-- Navbar Brand -->
        <a class="navbar-brand" href="#">
          <img src="../components/icons/book.png" alt="" /> Q'Cursos
        </a>

        <!-- Navbar Toggler -->
        <button
          class="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarSupportedContent"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>

        <!-- Collapse Menu -->
        <!-- Navbar Content -->
        <div class="collapse navbar-collapse" id="navbarSupportedContent">
          <ul class="navbar-nav ms-auto">
            <li class="nav-item undercolor">
              <a
                v-on:click="changeComponent('class')"
                class="nav-link"
                aria-current="page"
                href="#"
                >Turmas</a
              >
            </li>
            <li class="nav-item undercolor">
              <a v-on:click="changeComponent('tests')" class="nav-link" href="#"
                >Avaliações</a
              >
            </li>
            <li class="nav-item undercolor">
              <a
                v-on:click="changeComponent('attendances')"
                class="nav-link"
                href="#"
                >Presenças</a
              >
            </li>
            <li class="nav-item undercolor">
              <a v-on:click="generatePdfReport()" class="nav-link" href="#"
                >Boletim</a
              >
            </li>
            <li class="nav-item undercolor">
              <a v-on:click="generateFrquency()" class="nav-link" href="#"
                >Atestado Frequência</a
              >
            </li>
            <li class="nav-item">
              <button
                class="logout-button btn btn-outline-danger"
                v-on:click="logout()"
              >
                Sair
              </button>
            </li>
          </ul>
        </div>
      </div>
    </nav>

    <div v-if="actualComponent == 'class'" class="container">
      <ClassesComponent />
    </div>
    <div v-if="actualComponent == 'tests'" class="container">
      <TestsComponent />
    </div>
    <div v-if="actualComponent == 'attendances'" class="container">
      <AttendancesComponent />
    </div>
    <div v-if="actualComponent == 'courseReport'" class="container">
      <CourseReportsComponent />
    </div>
    <div v-if="actualComponent == 'certificates'" class="container">
      <CertificatesComponent />
    </div>
  </main>
</template>

<style scoped>
.main {
  min-height: 100vh;

  padding-bottom: 1rem;

  background-image: url("../../src/assets/background2.jpg");
  background-blend-mode: multiply;
  background-size: 100%;
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
.bg-darker {
  background-color: var(--dark-darker);
}

.logout-button {
  /* margin-left: 1rem; */
  width: 5rem;
}

.nav-item {
  margin: 0 5px;
}
</style>

<script>
import jsPDF from "jspdf";
import autoTable from "jspdf-autotable";
import ClassesComponent from "../components/StudentComponents/ClassesComponent.vue";
import TestsComponent from "../components/StudentComponents/TestsComponent.vue";
import CertificatesComponent from "../components/StudentComponents/CertificatesComponent.vue";
import CourseReportsComponent from "../components/StudentComponents/CourseReportsComponent.vue";
import AttendancesComponent from "../components/StudentComponents/AttendancesComponent.vue";
import api from "../services/api";

export default {
  name: "TeacherView",
  components: {
    ClassesComponent,
    TestsComponent,
    CertificatesComponent,
    CourseReportsComponent,
    AttendancesComponent,
  },
  data() {
    return {
      actualComponent: "",
      loggedTeacher: {
        username: "robson",
      },

      allAverages: [],
      institution: {},
    };
  },
  async beforeMount() {
    this.verifyPermission();
    this.actualComponent = "class";
    this.currentLogged = await JSON.parse(
      localStorage.getItem("@q-cursos:user")
    );
    this.allAverages = (
      await api.get(`/Averages/${this.currentLogged.id}`)
    ).data;
    this.institution = (await api.get("/Institution")).data.find(
      (i) => i.id == this.currentLogged.fk_institution
    );
  },
  methods: {
    async verifyPermission() {
      try {
        let user = localStorage.getItem("@q-cursos:user");

        if (!user) {
          this.$router.push("/");
          return;
        }

        user = JSON.parse(user);

        const response = await api.post("/Authentication/loginStudent", {
          registry: user.registry,
          born_date: user.born_date,
        });

        if (response.data.length == 0) {
          localStorage.clear();
          this.$router.push("/");
          return;
        }
      } catch (error) {
        this.$router.push("/");
      }
    },
    async logout() {
      localStorage.removeItem("@q-cursos:user");
      this.$router.push("/");
    },
    async changeComponent(target) {
      this.actualComponent = target;
    },
    generatePdfReport() {
      const doc = new jsPDF({ format: "" });
      var currentRow = 10;

      doc.setFontSize(11);

      autoTable(doc, {
        head: [[this.institution.name, new Date().toLocaleDateString()]],
        startY: currentRow,
      });

      // doc.text(
      //   `${this.institution.name}, ${new Date().toLocaleDateString()}`,
      //   5,
      //   currentRow
      // );
      currentRow += 10;

      autoTable(doc, {
        head: [["Aluno", "Matrícula"]],
        body: [[this.currentLogged.name, this.currentLogged.registry]],
        startY: currentRow,
      });

      // doc.text(`Aluno: ${this.currentLogged.name}`, 5, currentRow);
      currentRow += 20;

      const arr = this.allAverages.map((a) => {
        return [
          a.course_name,
          a.class_name,
          `${a.frequency}%`,
          a.grade,
          a.situation,
        ];
      });
      autoTable(doc, {
        head: [["Curso", "Turma", "Frequência", "Média", "Situação"]],
        body: arr,
        startY: currentRow,
      });
      doc.save(`Boletim (${this.currentLogged.name}).pdf`);
    },
    async generateFrquency() {
      var currentRow = 10;
      var response = await api.post(
        `/Frequency/Student?student=${this.currentLogged.id}`
      );

      response = response.data;

      const arr = response.map((f) => {
        return [f.course_name, f.class_name, `${f.frequencyrate}%`];
      });

      const doc = new jsPDF();
      doc.setFontSize(11);

      autoTable(doc, {
        head: [[this.institution.name, new Date().toLocaleDateString()]],
        startY: currentRow,
      });
      currentRow += 10;

      autoTable(doc, {
        head: [["Aluno", "Matrícula"]],
        body: [[this.currentLogged.name, this.currentLogged.registry]],
        startY: currentRow,
      });

      currentRow += 20;

      autoTable(doc, {
        head: [["Curso", "Turma", "Frequência"]],
        body: arr,
        startY: currentRow,
      });

      doc.save(
        `atestado_de_frequencia_(${this.currentLogged.name.replaceAll(
          " ",
          "_"
        )}).pdf`
      );
    },
  },
};
</script>
