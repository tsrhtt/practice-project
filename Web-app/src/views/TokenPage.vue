<template>
  <div class="app-container">
    <aside class="navigation">
      <div class="nav-item">
        <i class="fas fa-vial"></i>
        <span>Лаборатория</span>
      </div>
    </aside>

    <main class="content">
      <header class="header">
        <h1>ЛАБОРАТОРИЯ.WEB</h1>
      </header>

      <section class="tables">
        <div class="button-container">
          <button @click="getDirectionsFromDb">Получить направления</button>
          <button @click="confirmSync" class="sync-button">Синхронизироваться с ЛИС</button>
          <button v-if="directions.length > 0" @click="goToAnalyzersPage" class="analyzers-button">Анализаторы</button>
        </div>

        <h2>Запросы</h2>
        <table class="styled-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Полное имя пациента</th>
              <th>Лаборатория</th>
              <th>Тип анализа</th>
              <th>Статус</th>
              <th>Дата запроса</th>
              <th>Запрошено</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="request in requests" :key="request.id" @click="showDetails(request)">
              <td>{{ request.id }}</td>
              <td>{{ request.patientFullName }}</td>
              <td>{{ request.laboratory }}</td>
              <td>{{ request.analysTypeName }}</td>
              <td>{{ request.directionStatus }}</td>
              <td>{{ formatDate(request.requestDate) }}</td>
              <td>{{ request.requestedBy }}</td>
            </tr>
          </tbody>
        </table>

        <h2>Направления</h2>
        <table class="styled-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Полное имя пациента</th>
              <th>Лаборатория</th>
              <th>Тип анализа</th>
              <th>Статус</th>
              <th>Дата запроса</th>
              <th>Запрошено</th>
              <th>Дата принятия</th>
              <th>Принято</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="direction in filteredDirections"
              :key="direction.id"
              :style="rowStyle(direction)"
              @click="showDetails(direction)"
            >
              <td>{{ direction.id }}</td>
              <td>{{ direction.patientFullName }}</td>
              <td>{{ direction.laboratory }}</td>
              <td>{{ direction.analysTypeName }}</td>
              <td>{{ direction.directionStatus }}</td>
              <td>{{ formatDate(direction.requestDate) }}</td>
              <td>{{ direction.requestedBy }}</td>
              <td>{{ direction.acceptedDate ? formatDate(direction.acceptedDate) : 'Не принято' }}</td>
              <td>{{ direction.acceptedBy ? direction.acceptedBy : '-' }}</td>
            </tr>
          </tbody>
        </table>
      </section>

      <div v-if="loading" class="loading-overlay">
        <div class="loading-message">
          <div class="spinner"></div>
          <p>Пожалуйста, подождите...</p>
        </div>
      </div>

      <div v-if="showConfirmationOverlay" class="confirmation-overlay">
        <div class="confirmation-message">
          <h3>Подтверждение</h3>
          <p>Все данные будут перезагружены. Вы уверены?</p>
          <div class="confirmation-buttons">
            <button @click="confirmGetDirections" class="confirm-button">Да</button>
            <button @click="cancelConfirmation" class="cancel-button">Отмена</button>
          </div>
        </div>
      </div>

      <PatientDetailsOverlay
        v-if="showOverlay"
        :direction="selectedDirection"
        :isEditing="isEditing"
        @close="showOverlay = false"
        @fetch-detailed-data="fetchDetailedData"
        @edit-direction="editDirection"
        @save-changes="saveChanges"
        @delete-direction="deleteDirection"
      />
    </main>
  </div>
</template>

<script>
import PatientDetailsOverlay from '../components/PatientDetailsOverlay.vue';

export default {
  components: {
    PatientDetailsOverlay,
  },
  data() {
    return {
      directions: [],
      loading: false,
      showOverlay: false,
      showConfirmationOverlay: false,
      selectedDirection: null,
      isEditing: false
    };
  },
  computed: {
    requests() {
      return this.directions.filter(direction => direction.directionStatusId === 1);
    },
    filteredDirections() {
      return this.directions.filter(direction => direction.directionStatusId !== 1);
    },
  },
  methods: {
    async getDirections() {
      this.loading = true;
      try {
        const response = await this.$api.get('direction');
        this.directions = this.processApiResponse(response.data);
      } catch (error) {
        console.error('Не удалось получить направления:', error);
        alert('Не удалось получить направления. Пожалуйста, проверьте консоль для получения подробной информации.');
      } finally {
        this.loading = false;
      }
    },
    async getDirectionsFromDb() {
      this.loading = true;
      try {
        const response = await this.$api.get('direction/fromdb');
        this.directions = this.processApiResponse(response.data); // Assuming response data is in the correct format
      } catch (error) {
        console.error('Не удалось получить направления из БД:', error);
        alert('Не удалось получить направления из БД. Пожалуйста, проверьте консоль для получения подробной информации.');
      } finally {
        this.loading = false;
      }
    },
    confirmSync() {
      this.showConfirmationOverlay = true;
    },
    confirmGetDirections() {
      this.showConfirmationOverlay = false;
      this.getDirections();
    },
    cancelConfirmation() {
      this.showConfirmationOverlay = false;
    },
    processApiResponse(data) {
      return data.$values.map(direction => {
        return {
          ...direction,
          directionStatusHistory: direction.directionStatusHistory.$values.reverse(),
        };
      });
    },
    formatDate(date) {
      const options = { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' };
      return new Date(date).toLocaleDateString('ru-RU', options);
    },
    rowStyle(direction) {
      if (direction.directionStatusId === 3) {
        return { backgroundColor: '#CD4A4C66' };
      } else if (direction.directionStatusId === 7) {
        return { backgroundColor: '#98FB9866' };
      }
      return {};
    },
    showDetails(direction) {
      this.selectedDirection = direction;
      this.isEditing = false; // Ensure editing mode is off when details are shown
      this.showOverlay = true;
    },
    async fetchDetailedData(directionId) {
      try {
        const response = await this.$api.get(`direction/detailed/${directionId}`);
        const detailedData = response.data;
        this.$router.push({
          name: 'DetailedPage',
          params: { id: directionId },
          query: { detailedData: JSON.stringify(detailedData) }
        });
      } catch (error) {
        console.error('Failed to fetch detailed data:', error);
        alert('Не удалось получить подробные данные.');
      }
    },
    editDirection() {
      this.isEditing = true;
    },
    async saveChanges(updatedDirection) {
      try {
        const response = await this.$api.put(`direction/${updatedDirection.id}`, updatedDirection);
        this.selectedDirection = response.data;
        this.isEditing = false;
        this.showOverlay = false;
        await this.getDirectionsFromDb(); // Refresh the directions after saving
      } catch (error) {
        console.error('Failed to save changes:', error);
        alert('Не удалось сохранить изменения.');
      }
    },
    async deleteDirection(directionId) {
      try {
        await this.$api.delete(`direction/${directionId}`);
        this.isEditing = false;
        this.showOverlay = false;
        await this.getDirectionsFromDb(); // Refresh the directions after deletion
      } catch (error) {
        console.error('Failed to delete direction:', error);
        alert('Не удалось удалить направление.');
      }
    },
    async acceptDirection(updatedDirection) {
      try {
        const request = {
          acceptedBy: updatedDirection.acceptedBy,
          comment: updatedDirection.laborantComment
        };
        await this.$api.post(`direction/accept/${updatedDirection.id}`, request);
        this.showOverlay = false;
        await this.getDirectionsFromDb(); // Refresh the directions after accepting
      } catch (error) {
        console.error('Failed to accept direction:', error);
        alert('Не удалось принять направление.');
      }
    },
    goToAnalyzersPage() {
      this.$router.push({ name: 'AnalyzersPage' });
    }
  },
};
</script>

<style scoped>
.app-container {
  display: flex;
  height: 100vh;
  font-family: 'Arial', sans-serif;
}

.navigation {
  width: 80px;
  background-color: #ADD8E6;
  padding: 20px 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
}

.nav-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-bottom: 20px;
  color: white;
  font-size: 14px;
}

.nav-item i {
  font-size: 24px;
  margin-bottom: 5px;
}

.content {
  flex-grow: 1;
  padding: 20px;
  margin-left: 100px;
  background-color: #f4f7f6;
  border-top-left-radius: 20px;
  overflow-y: auto;
}

.header {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 20px;
  height: 50px;
}

.header h1 {
  color: #ADD8E6;
  font-size: 24px;
  font-weight: bold;
}

.button-container {
  display: flex;
  justify-content: flex-start;
  margin-bottom: 20px;
}

.button-container button {
  background-color: #ADD8E6;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 20px;
  cursor: pointer;
  font-size: 16px;
  margin-right: 20px;
}

.button-container .sync-button {
  background-color: #CD4A4C66;
}

.button-container button:hover {
  background-color: #8dbcd4;
}

.button-container .sync-button:hover {
  background-color: #b33a3a;
}

.tables h2 {
  color: #ADD8E6;
  font-size: 18px;
  font-weight: bold;
  margin-top: 0;
  font-family: 'Arial', sans-serif;
}

.tables {
  background-color: white;
  padding: 20px;
  border-radius: 20px;
}

.styled-table {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 20px;
  border-radius: 20px;
  overflow: hidden;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
}

.styled-table th,
.styled-table td {
  padding: 12px 15px;
  text-align: left;
}

.styled-table th {
  background-color: #ADD8E6;
  color: white;
}

.styled-table tbody tr:nth-of-type(even) {
  background-color: #f3f3f3;
}

.styled-table tbody tr {
  border-bottom: 1px solid #dddddd;
}

.styled-table tbody tr:last-of-type {
  border-bottom: 2px solid #ADD8E6;
}

.loading-overlay,
.confirmation-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.loading-message,
.confirmation-message {
  background: white;
  width: 300px;
  padding: 20px;
  border-radius: 10px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
}

.spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #ADD8E6;
  border-top: 4px solid white;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 10px;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

.confirmation-message h3 {
  margin-bottom: 10px;
  font-size: 22px;
  font-weight: bold;
  color: #333;
}

.confirmation-message p {
  margin-bottom: 20px;
  font-size: 16px;
  color: #666;
}

.confirmation-buttons {
  display: flex;
  justify-content: space-between;
  width: 100%;
}

.confirmation-buttons button {
  width: 45%;
  padding: 10px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;
}

.confirmation-buttons .confirm-button {
  background-color: #ADD8E6;
}

.confirmation-buttons .cancel-button {
  background-color: #ffcccc;
}

.confirmation-buttons .confirm-button:hover {
  background-color: #8dbcd4;
}

.confirmation-buttons .cancel-button:hover {
  background-color: #ff9999;
}

.analyzers-button {
  background-color: #ADD8E6;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 20px;
  cursor: pointer;
  font-size: 16px;
  margin-right: 20px;
}

.analyzers-button:hover {
  background-color: #8dbcd4;
}
</style>
