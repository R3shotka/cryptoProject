import React, { SyntheticEvent } from 'react'

interface Props {
    onUserAssetsBalanceDelete: (e: SyntheticEvent) => void;
    UserAssetsBalanceValue: string;
}

const DeleteUserAssetsBalance = ({ onUserAssetsBalanceDelete, UserAssetsBalanceValue }: Props) => {
    return (
        <div>
            <form onSubmit={onUserAssetsBalanceDelete} >
                <input hidden={true} value={UserAssetsBalanceValue} readOnly={true} />

                {/* 2. Додали кнопку, яка запускає процес видалення! */}
                <button className="block w-full py-3 text-white duration-200 border-2 rounded-lg bg-red-500 hover:text-red-500 hover:bg-white border-red-500">
                    remove
                </button>
            </form>
        </div>
    )
}

export default DeleteUserAssetsBalance