import {createApp} from 'vue'
import App from './App.vue'
import router from './router'
import {library} from "@fortawesome/fontawesome-svg-core";
import {faGamepad, faUserCheck, faUserMinus, faUserPlus} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/vue-fontawesome";
import {IconDefinition} from '@fortawesome/free-brands-svg-icons';

library.add(
    faGamepad as IconDefinition,
    faUserCheck as IconDefinition,
    faUserPlus as IconDefinition,
    faUserMinus as IconDefinition
);


createApp(App).use(router).component("font-awesome-icon", FontAwesomeIcon).mount('#app')