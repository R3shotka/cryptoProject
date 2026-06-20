import React from 'react'

// Вказуємо, що компонент чекає пропсу coinName
interface Props {
    coinName: string;
}

// Витягуємо coinName з пропсів (деструктуризація)
const Sidebar = ({ coinName }: Props) => {
    return (
        <nav className="block py-4 px-6 top-0 bottom-0 w-64 bg-white shadow-xl left-0 absolute flex-row flex-nowrap md:z-10 z-50 transition-all duration-300 ease-in-out transform md:translate-x-0 -translate-x-full">
            <div className="flex-col min-h-full px-0 flex flex-wrap items-center justify-between w-full mx-auto overflow-y-auto overflow-x-hidden">
                <div className="flex bg-white flex-col items-stretch opacity-100 relative mt-4 overflow-y-auto overflow-x-hidden h-auto z-40 items-center flex-1 rounded w-full">
                    <div className="md:flex-col md:min-w-full flex flex-col list-none">
                        <h6 className="md:min-w-full text-slate-500 text-xs uppercase font-bold block pt-1 pb-4 no-underline">
                            {/* Підставляємо нашу пропсу */}
                            Дашборд монети: {coinName}
                        </h6>
                    </div>
                </div>
            </div>
        </nav>
    )
}

export default Sidebar