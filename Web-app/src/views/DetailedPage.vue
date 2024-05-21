<template>
  <div class="detailed-page">
    <aside class="navigation">
      <div class="nav-item" @click="goToMainPage">
        <i class="fas fa-arrow-left"></i>
        <span>Назад</span>
      </div>
      <div class="nav-item">
        <i class="fas fa-vial"></i>
        <span>Лаборатория</span>
      </div>
    </aside>

    <main class="content">
      <header class="header">
        <h1>ЛАБОРАТОРИЯ.WEB</h1>
      </header>

      <div class="details-container">
        <div class="left-panel">
          <div class="patient-info">
            <h2>Пациент</h2>
            <p><strong>Полное имя:</strong> {{ detailedData.patient.fullName || 'Нет данных' }}</p>
            <p><strong>Пол:</strong> {{ detailedData.patient.sexDescription || 'Нет данных' }}</p>
            <p><strong>Возраст:</strong> {{ detailedData.patient.age || 'Нет данных' }}</p>
            <p><strong>Дата рождения:</strong> {{ formatDate(detailedData.patient.birthDate) || 'Нет данных' }}</p>
            <p><strong>Номер идентификации:</strong> {{ detailedData.patient.identificationNumber || 'Нет данных' }}</p>
          </div>

          <div class="direction-info">
            <h2>Данные направления</h2>
            <p><strong>ID:</strong> {{ detailedData.id || 'Нет данных' }}</p>
            <p><strong>Полное имя пациента:</strong> {{ detailedData.patientFullName || 'Нет данных' }}</p>
            <p><strong>Лаборатория:</strong> {{ detailedData.laboratory || 'Нет данных' }}</p>
            <p><strong>Тип анализа:</strong> {{ detailedData.analysTypeName || 'Нет данных' }}</p>
            <p><strong>Статус:</strong> {{ detailedData.directionStatus || 'Нет данных' }}</p>
            <p><strong>Дата запроса:</strong> {{ formatDate(detailedData.requestDate) || 'Нет данных' }}</p>
            <p><strong>Запрошено:</strong> {{ detailedData.requestedBy || 'Нет данных' }}</p>
            <p><strong>Дата принятия:</strong> {{ detailedData.acceptedDate ? formatDate(detailedData.acceptedDate) : 'Нет данных' }}</p>
            <p><strong>Принято:</strong> {{ detailedData.acceptedBy || 'Нет данных' }}</p>
            <p><strong>Дата выполнения:</strong> {{ detailedData.readyDate ? formatDate(detailedData.readyDate) : 'Нет данных' }}</p>
            <p><strong>Комментарии:</strong> {{ detailedData.laborantComment || 'Нет данных' }}</p>
            <p><strong>Тип биоматериала:</strong> {{ detailedData.bioMaterialType || 'Нет данных' }}</p>
          </div>

          <div class="direction-history">
            <h2>История статусов направления</h2>
            <div class="history-items">
              <div v-for="history in detailedData.directionStatusHistory.$values" :key="history.id" class="history-item">
                <p><strong>Статус:</strong> {{ getStatusDescription(history.directionStatusId) }}</p>
                <p><strong>Дата и время:</strong> {{ formatDate(history.dateTime) }}</p>
                <p><strong>Пользователь:</strong> {{ history.userFio }}</p>
                <p><strong>Комментарий:</strong> {{ history.comment || 'Нет данных' }}</p>
              </div>
            </div>
          </div>
        </div>

        <div class="right-panel">
          <div v-if="detailedData.directionStatusId === 3" class="warning-message">
            <svg class="warning-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M10.29 3.86L1.82 18a2 2 0 001.71 3h16.94a2 2 0 001.71-3L13.71 3.86a2 2 0 00-3.42 0zM12 9v4m0 4h.01"/>
            </svg>
            Внимание: биоматериал отсутствует
          </div>

          <div v-for="group in indicatorGroups" :key="group.groupOrderNumber" class="indicator-group" :class="{'border-red': detailedData.directionStatusId === 3}">
            <h2>{{ group.group }}</h2>
            <div v-for="indicator in group.indicators" :key="indicator.id" class="indicator-item">
              <div class="indicator-title">
                <span v-if="indicator.resultVal !== null || indicator.resultStr !== null" class="status-icon green"></span>
                <span v-else-if="detailedData.directionStatusId === 7 && indicator.resultVal === null && indicator.resultStr === null" class="status-icon red"></span>
                <span v-else-if="indicator.resultVal === null && indicator.resultStr === null" class="status-icon orange"></span>
                {{ indicator.name }}
              </div>
              <p v-if="indicator.units"><strong>Единицы:</strong> {{ indicator.units }}</p>
              <p v-if="indicator.type"><strong>Тип:</strong> {{ indicator.type }}</p>
              <p v-if="indicator.comment"><strong>Комментарий:</strong> {{ indicator.comment }}</p>
              <p v-if="indicator.isAdditional !== null"><strong>Дополнительный:</strong> {{ indicator.isAdditional ? 'Да' : 'Нет' }}</p>
              <p v-if="indicator.isNormExist !== null"><strong>Норма существует:</strong> {{ indicator.isNormExist ? 'Да' : 'Нет' }}</p>
              <p v-if="indicator.minStandardValue !== null"><strong>Минимальное значение нормы:</strong> {{ indicator.minStandardValue }}</p>
              <p v-if="indicator.maxStandardValue !== null"><strong>Максимальное значение нормы:</strong> {{ indicator.maxStandardValue }}</p>
              <p v-if="indicator.resultVal !== null"><strong>Результат:</strong> {{ indicator.resultVal }}</p>
              <p v-if="indicator.resultStr !== null"><strong>Результат (строка):</strong> {{ indicator.resultStr }}</p>
              <p v-if="indicator.textStandards && indicator.textStandards.length"><strong>Текстовые стандарты:</strong> {{ indicator.textStandards.join(', ') }}</p>
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script>
export default {
  props: {
    detailedData: {
      type: Object,
      required: true,
    },
  },
  computed: {
    indicatorGroups() {
      const groups = {};
      if (this.detailedData.indicators && this.detailedData.indicators.$values) {
        this.detailedData.indicators.$values.forEach(indicator => {
          if (!groups[indicator.groupOrderNumber]) {
            groups[indicator.groupOrderNumber] = {
              group: indicator.group,
              groupOrderNumber: indicator.groupOrderNumber,
              indicators: []
            };
          }
          groups[indicator.groupOrderNumber].indicators.push(indicator);
        });
      }
      return Object.values(groups).sort((a, b) => a.groupOrderNumber - b.groupOrderNumber);
    }
  },
  methods: {
    goToMainPage() {
      this.$router.push({ name: 'TokenPage' });
    },
    formatDate(date) {
      const options = { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' };
      return new Date(date).toLocaleDateString('ru-RU', options);
    },
    getStatusDescription(statusId) {
      const statusDescriptions = {
        1: 'Заявка',
        2: 'В ожидании',
        3: 'Нет биоматериала',
        4: 'Отказ',
        5: 'Отменено',
        6: 'В работе',
        7: 'Готово',
      };
      return statusDescriptions[statusId] || 'Неизвестный статус';
    },
  },
};
</script>

<style scoped>
.detailed-page {
  display: flex;
  flex-direction: column;
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
  cursor: pointer;
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

.details-container {
  display: flex;
}

.left-panel {
  flex: 1;
  margin-right: 20px;
}

.right-panel {
  flex: 2;
}

.patient-info, .direction-info, .direction-history {
  background-color: white;
  padding: 20px;
  border-radius: 20px;
  margin-bottom: 20px;
}

.warning-message {
  color: red;
  display: flex;
  align-items: center;
  font-weight: bold;
  margin-bottom: 20px;
}

.warning-message .warning-icon {
  width: 20px;
  height: 20px;
  margin-right: 10px;
}

.indicator-group {
  background-color: white;
  padding: 20px;
  border-radius: 20px;
  margin-bottom: 20px;
}

.indicator-item {
  border-top: 1px solid rgba(0, 0, 0, 0.1);
  padding: 10px 0;
}

.indicator-item:first-of-type {
  border-top: none;
}

.indicator-title {
  font-weight: bold;
  display: flex;
  align-items: center;
}

.status-icon {
  display: inline-block;
  width: 10px;
  height: 10px;
  border-radius: 50%;
  margin-right: 5px;
}

.status-icon.green {
  background-color: green;
}

.status-icon.orange {
  background-color: orange;
}

.status-icon.red {
  background-color: red;
}

.border-red {
  border: 1px solid red;
}

.history-items {
  display: flex;
  flex-wrap: wrap;
}

.history-item {
  background-color: #b0e0e6;
  border-radius: 10px;
  padding: 10px;
  margin: 5px;
  flex: 1;
}
</style>