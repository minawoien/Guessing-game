<template>
    <!--startgame choices, shows if ....-->
    <div v-if="startChoices" style="width:330px; padding-top:100px;">
        <div v-if="!loggedIn" class="d-grid gap-3">
            <button
                type="button"
                class="btn btn-lg btn-primary shadow-lg"
                @click="checkLogin"
            >
                Play
            </button>
            <button
                type="button"
                class="btn btn-lg btn-danger shadow-lg"
                @click="$router.push('/recentgames')"
            >
                Recent Games
            </button>
            <button
                type="button"
                class="btn btn-lg btn-success shadow-lg"
                @click="$router.push('/leaderboard')"
            >
                Leaderboard
            </button>
        </div>
        <div v-if="loggedIn">
            <form method="POST" v-on:submit.prevent="joinLobby">
                <h4 class="mb-3">How would you like to play?</h4>
                <div class="bg-white p-3 shadow-lg rounded-3">
                    <div class="form-check">
                        <input
                            name="gametype"
                            class="form-check-input"
                            type="radio"
                            value="0"
                            checked
                            v-model="lobbyChoices.gametype"
                            v-on:change="checkForm"
                        />
                        <label class="form-check-label" for="one-player"
                        >Single Player</label
                        >
                    </div>
                    <div class="form-check">
                        <input
                            name="gametype"
                            class="form-check-input"
                            type="radio"
                            value="1"
                            v-model="lobbyChoices.gametype"
                            v-on:change="checkForm"
                        />
                        <label class="form-check-label" for="two-player"
                        >Two Player</label
                        >
                    </div>
                    <div class="form-check">
                        <input
                            name="gametype"
                            class="form-check-input"
                            type="radio"
                            value="2"
                            v-model="lobbyChoices.gametype"
                            v-on:change="checkForm"
                        />
                        <label class="form-check-label" for="multi-player"
                        >Multi Player</label
                        >
                    </div>
                </div>
                <div v-if="showChoiceOne">
                    <h5 class="my-3">Join a game or start a new?</h5>
                    <div class="bg-white p-3 shadow-lg rounded-3">
                        <div class="form-check">
                            <input
                                name="game"
                                class="form-check-input"
                                type="radio"
                                value="join"
                                checked
                                v-model="lobbyChoices.game"
                                v-on:change="checkForm"
                            />
                            <label class="form-check-label" for="guesser"
                            >Join</label
                            >
                        </div>
                        <div class="form-check">
                            <input
                                name="game"
                                class="form-check-input"
                                type="radio"
                                value="new"
                                v-model="lobbyChoices.game"
                                v-on:change="checkForm"
                            />
                            <label class="form-check-label" for="proposer"
                            >New</label
                            >
                        </div>
                    </div>
                </div>
                <div v-if="showChoiceTwo">
                    <h5 class="my-3">Which role?</h5>
                    <div class="bg-white p-3 shadow-lg rounded-3">
                        <div class="form-check">
                            <input
                                name="role"
                                class="form-check-input"
                                type="radio"
                                value="0"
                                checked
                                v-model="lobbyChoices.role"
                            />
                            <label class="form-check-label" for="guesser"
                            >Guesser</label
                            >
                        </div>
                        <div class="form-check">
                            <input
                                name="role"
                                class="form-check-input"
                                type="radio"
                                value="1"
                                v-model="lobbyChoices.role"
                            />
                            <label class="form-check-label" for="proposer"
                            >Proposer</label
                            >
                        </div>
                    </div>
                </div>
                <button class="btn btn-lg w-100 btn-primary my-3" type="submit">
                    Play
                </button>
            </form>
        </div>
    </div>

    <lobby
        v-if="activeComponents.lobby"
        v-bind:lobbyChoices="lobbyChoices"
        @startGame="startGame"
        @joinStartedGame="joinStartedGame"
        v-bind:userName="userName"
        @leftLobby="leftLobby"
    />
    <lobbies
        v-if="activeComponents.lobbies"
        v-bind:lobbyChoices="lobbyChoices"
        @getJoinedLobby="getJoinedLobby"
        v-bind:userName="userName"
    />
</template>

<script lang="ts">
import {defineComponent} from "vue";

import Lobby from "@/components/game/Lobby.vue";
import Lobbies from "@/components/game/Lobbies.vue";
import * as Lob from "../../domain/game/Lobby";

export default defineComponent({
    name: "PreGame",
    props: ["userName"],
    components: {
        Lobbies,
        Lobby
    },
    emits: ["startGame", "joinStartedGame"],
    data: function () {
        return {
            startChoices: true,
            loggedIn: false,
            activeComponents: {
                lobbies: false,
                lobby: false
            },
            lobbyChoices: {
                gametype: 0,
                game: "join",
                role: 0,
                gameId: 0,
                lobbyId: 0
            } as Lob.LobbyChoices,
            showChoiceOne: false,
            showChoiceTwo: false,
            //lobbyData: new Object() as Lob.LobbyData,
        };
    },
    methods: {
        leftLobby: function () {
            this.startChoices = true;
            this.activeComponents.lobby = false;

        },
        checkForm: function () {
            if (
                this.lobbyChoices.gametype == 1 ||
                this.lobbyChoices.gametype == 2
            ) {
                this.showChoiceOne = true;
            } else {
                this.showChoiceOne = false;
            }
            if (this.lobbyChoices.gametype == 2 && this.lobbyChoices.game == "new") {
                this.showChoiceTwo = true;
            } else {
                this.showChoiceTwo = false;
            }
        },
        checkLogin: function () {
            if (this.userName) {
                this.loggedIn = !this.loggedIn;
            } else {
                this.$router.push("/login");
            }
        },

        postLobby: async function () {
            var response = await Lob.postLobby({
                Id: this.lobbyChoices.lobbyId,
                Type: this.lobbyChoices.gametype,
                Role: this.lobbyChoices.role
            })
            if (response.status === 200) {
                this.lobbyChoices.lobbyId = response.body.data.lobbyId
            }
        },
        startGame: async function () {
            this.$emit("startGame");
        },
        joinStartedGame: async function () {
            this.$emit("joinStartedGame")
        },
        getJoinedLobby: function () {
            this.activeComponents.lobby = true;
            this.activeComponents.lobbies = false;
            this.startChoices = false;
        },

        joinLobby: async function () {
            this.startChoices = false;
            if (this.lobbyChoices.gametype == 0) {
                await this.postLobby();
                await this.startGame();
            } else if (this.lobbyChoices.gametype == 1) {
                if (this.lobbyChoices.game == "new") {
                    this.lobbyChoices.role = 1
                    await this.postLobby();
                    this.activeComponents.lobby = true;
                } else {
                    this.activeComponents.lobbies = true;
                }
            } else if (this.lobbyChoices.gametype == 2) {
                if (this.lobbyChoices.game == "new") {
                    await this.postLobby();
                    this.activeComponents.lobby = true;
                } else {
                    this.activeComponents.lobbies = true;
                }
            } else {
                this.startChoices = true;
            }
        },
        checkIfActiveLobbyOrLobbies: async function () {
            let response = await Lob.getLobby();
            if (response.status === 200) {
                this.activeComponents.lobby = true
                this.startChoices = false
            } else if (this.lobbyChoices.gametype > 0) {
                let response = await Lob.getLobbiesByType(this.lobbyChoices.gametype);
                if (response.status === 200) {
                    this.activeComponents.lobbies = true
                    this.startChoices = false
                }
            } else {
                this.startChoices = true;
            }

        },
    },
    created: async function () {
        await this.checkIfActiveLobbyOrLobbies()
    }
});
</script>
