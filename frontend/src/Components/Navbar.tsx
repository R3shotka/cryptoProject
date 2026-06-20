import React from 'react'
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../Context/AuthContext';

interface Props { }

const Navbar = (props: Props) => {
    const { isAuthenticated, logout, user } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <nav className="bg-white shadow-md sticky top-0 z-50">
            <div className="container mx-auto px-6 py-4">
                <div className="flex items-center justify-between">
                    <Link to="/" className="flex items-center gap-2">
                        <div className="text-3xl font-extrabold text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-purple-600">
                            CryptoProject
                        </div>
                    </Link>

                    {isAuthenticated && (
                        <div className="hidden md:flex items-center gap-8">
                            <Link
                                to="/search"
                                className="text-gray-700 hover:text-blue-600 font-medium transition-colors"
                            >
                                Search
                            </Link>
                            <Link
                                to="/portfolio"
                                className="text-gray-700 hover:text-blue-600 font-medium transition-colors"
                            >
                                Portfolio
                            </Link>
                        </div>
                    )}

                    <div className="flex items-center gap-4">
                        {isAuthenticated ? (
                            <>
                                <span className="text-gray-600 text-sm hidden sm:block">
                                    👋 {user?.userName}
                                </span>
                                <button
                                    onClick={handleLogout}
                                    className="px-6 py-2 font-medium rounded-lg text-white bg-gradient-to-r from-red-500 to-red-600 hover:shadow-lg transition-all"
                                >
                                    Logout
                                </button>
                            </>
                        ) : (
                            <>
                                <Link
                                    to="/login"
                                    className="text-gray-700 hover:text-blue-600 font-medium"
                                >
                                    Login
                                </Link>
                                <Link
                                    to="/register"
                                    className="px-6 py-2 font-medium rounded-lg text-white bg-gradient-to-r from-blue-600 to-purple-600 hover:shadow-lg transition-all"
                                >
                                    Sign Up
                                </Link>
                            </>
                        )}
                    </div>
                </div>
            </div>
        </nav>
    )
}

export default Navbar