<template>
    <div class="row">
        <div class="w-auto mx-auto">
            <pre-game
                v-if="activeComponents.preGame"
                @startGame="startGame"
                @joinStartedGame="joinStartedGame"
                v-bind:userName="userName"
            />
            <player v-if="activeComponents.player" v-bind:userName="userName" @quitGame="quitGame"/>
        </div>
    </div>
</template>

<script lang="ts">
import {defineComponent} from "vue";
import PreGame from "@/components/game/PreGame.vue";
import Player from "@/components/game/Player.vue";
import * as Lob from "../../domain/game/Lobby";
import * as Game from "../../domain/game/Game";

export default defineComponent({
    name: "Game",
    components: {
        PreGame,
        Player
    },
    emits: ["startGame", "quitGame", "joinStartedGame"],
    props: ["userName"],
    data: function () {
        return {
            activeComponents: {
                preGame: true,
                player: false
            },
            lobbyChoices: new Object() as Lob.LobbyChoices
        };
    },
    methods: {
        quitGame: function () {
            this.activeComponents.player = false;
            this.activeComponents.preGame = true;
        },
        startGame: async function () {
            let response = await Lob.startGame()
            if (response == 200) {
                this.joinStartedGame();
            } else {
                this.activeComponents.preGame = true
            }


        },
        joinStartedGame: function () {
            this.activeComponents.preGame = false;
            this.activeComponents.player = true;

        },
        getGame: async function () {
            let response = await Game.getGame();
            if (response.status == 200) {
                this.activeComponents.player = true;
                this.activeComponents.preGame = false;
            }
        },

    },
    created: async function () {
        await this.getGame();
    }

});
</script>
