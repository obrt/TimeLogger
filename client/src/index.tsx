import * as React from "react";
import * as ReactDOM from "react-dom";
import Application from "./app/App";
import { DeveloperProvider } from "./app/DeveloperContext";

ReactDOM.render(
    <DeveloperProvider>
        <Application />
    </DeveloperProvider>,
    document.getElementById("root")
);
