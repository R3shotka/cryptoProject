import App from "../App";
import SearchPage from "../Pages/SearchPage/SearchPage";
import HomePage from "../Pages/HomePage/HomePage";
import LoginPage from "../Pages/LoginPage/LoginPage";
import RegisterPage from "../Pages/RegisterPage/RegisterPage";
import PortfolioPage from "../Pages/PortfolioPage/PortfolioPage";
import { createBrowserRouter } from "react-router-dom";
import CoinPage from "../Pages/CoinPage/CoinPage";
import ProtectedRoute from "../Components/ProtectedRoute/ProtectedRoute";

export const router = createBrowserRouter([
    {
        path: "/",
        element: <App />,
        children: [
            { path: "", element: <HomePage /> },
            { path: "login", element: <LoginPage /> },
            { path: "register", element: <RegisterPage /> },
            {
                path: "search",
                element: (
                    <ProtectedRoute>
                        <SearchPage />
                    </ProtectedRoute>
                )
            },
            {
                path: "portfolio",
                element: (
                    <ProtectedRoute>
                        <PortfolioPage />
                    </ProtectedRoute>
                )
            },
            {
                path: "crypto/:id",
                element: (
                    <ProtectedRoute>
                        <CoinPage />
                    </ProtectedRoute>
                )
            }
        ]
    },
]);