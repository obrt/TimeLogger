import * as React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Projects from "./views/Projects";
import Timelogs from "./views/Timelogs";
import "./style.css";
localStorage.setItem('developerId', '1');

export default function App() {
    return (
        <Router>
            <>
                <header className="bg-gray-900 text-white flex items-center h-12 w-full">
                    <div className="container mx-auto flex justify-between items-center">
                        <a className="navbar-brand hover:text-gray-300 hover:underline" href="/">
                            Timelogger App
                        </a>
                        <div></div>
                        <nav className="space-x-8">
                        <a href="/projects" className="hover:text-gray-300 hover:underline">
                            Projects
                        </a>
                        <a href="/timelogs" className="hover:text-gray-300 hover:underline">
                            Timelogs
                        </a>
                        </nav>
                    </div>
                </header>

                <main>
                    <div className="container mx-auto">
                        <Routes>
                            <Route path="/" element={<Projects />} />
                            <Route path="/timelogs" element={<Timelogs />} />
                            <Route path="/projects" element={<Projects />} />
                        </Routes>
                    </div>
                </main>
            </>
        </Router>
    );
}
