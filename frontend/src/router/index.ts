import {createRouter, createWebHashHistory, RouteRecordRaw} from 'vue-router'
import Game from "../components/game/Game.vue"
import LeaderBoard from "../components/result/LeaderBoard.vue"
import Login from "../components/auth/Login.vue"
import Register from "../components/auth/Register.vue"
import RecentGames from "../components/result/RecentGames.vue"


const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        name: 'Home',
        component: Game
    },
    {
        path: '/login',
        name: 'Login',
        component: Login
    },
    {
        path: '/register',
        name: 'Register',
        component: Register
    },
    {
        path: '/leaderboard',
        name: 'LeaderBoard',
        component: LeaderBoard
    },
    {
        path: '/recentgames',
        name: 'RecentGames',
        component: RecentGames
    }

]

const router = createRouter({
    history: createWebHashHistory(process.env.BASE_URL),
    routes
})

export default router
