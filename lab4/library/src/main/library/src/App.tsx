import React from 'react';
import './App.css';
import {Navigation} from "./Navigation/Navigation";
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min'
import {Books} from "./pages/Books";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import {Register} from "./pages/Register";
import {Borrowings} from "./pages/Borrowings";
import {Reservations} from "./pages/Reservations";
import {Homepage} from "./pages/Homepage";
import {ProfilePage} from "./pages/ProfilePage";
import {NewBookPage} from "./pages/NewBookPage";

function App() {
    return (
        <BrowserRouter>
            <Navigation/>
            <div className="container pb-3">
                <Routes>
                    <Route path={"/books"} element={<Books/>}/>
                    <Route path={"/reservations"} element={<Reservations/>}/>
                    <Route path={"/borrowings"} element={<Borrowings/>}/>
                    <Route path={"/register"} element={<Register/>}/>
                    <Route path={"/profile"} element={<ProfilePage/>}/>
                    <Route path={"/newBook"} element={<NewBookPage/>}/>
                    <Route path={"/"} element={<Homepage/>}/>
                    <Route path={"*"} element={<>Not found!</>}/>
                </Routes>
            </div>
        </BrowserRouter>
    );
}

export default App;
