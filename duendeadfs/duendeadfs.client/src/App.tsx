import { Outlet } from "react-router-dom";
import "./App.css";
import Header from "./components/main/Header";

function App() {
  return (
    <div>
      <Header />
      <main>
        <div className="mx-auto max-w-7xl py-6 sm:px-6 lg:px-8">
            <Outlet />
        </div>
      </main>
    </div>
  );
}

export default App;
