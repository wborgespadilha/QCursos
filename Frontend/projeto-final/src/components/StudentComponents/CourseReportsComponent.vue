<script setup>
import jsPDF from "jspdf";
import autoTable from "jspdf-autotable";
import api from "../../services/api";
</script>

<template>
  <main>
    <h1 class="title">Boletim</h1>
    <hr />
    <button class="theButton" v-on:click="generatePdf()">Gerar Boletim</button>
  </main>
</template>

<style scoped>

.theButton {
  border: unset;

  border-radius: 10px;

  margin: 1rem auto;

  box-shadow: 5px 5px #252525;
  transition: 0.1s;

  background-color: #08aeea;
  background-image: linear-gradient(0deg, #08aeea 0%, #2af598 100%);
}
.theButton:hover {
  background-color: #2af598;
  background-image: linear-gradient(0deg, #2af598 0%, #08aeea 100%);
}
.theButton:active {
  transform: translate(5px, 5px);
  box-shadow: 0px 0px;
  transition: .1s;
}
main {
  margin-top: 25px;
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
  name: "CourseReportsComponent",
  async beforeMount() {
    this.currentLogged = await JSON.parse(
      localStorage.getItem("@q-cursos:user")
    );
    this.allAverages = (await api.get(`/Averages/${this.currentLogged.id}`)).data;
    this.institution = (await api.get('/Institution')).data.find( i => i.id == this.currentLogged.fk_institution);
  },
  data() {
    return {
      currentLogged: {},
      allAverages: [],
      institution: {},
    };
  },
  methods: {
    generatePdf() {
      const doc = new jsPDF();
      var currentRow = 10;

      doc.setFontSize(11);

      doc.text(`${this.institution.name}, ${new Date().toLocaleDateString()}`, 5, currentRow);
      currentRow += 10;

      doc.text(`Aluno: ${this.currentLogged.name}`, 5, currentRow);
      currentRow += 20;

      const arr = this.allAverages.map(a => {
        return [a.course_name, a.class_name, `${a.frequency}%`, a.grade, a.situation];
      });
      autoTable(doc, {
        head: [["Curso", "Turma", "Frequência", 'Média', 'Situação']],
        body: arr,
        startY: currentRow,
      });
      doc.save(`Boletim (${this.currentLogged.name}).pdf`);
    },
  },
};
</script>
