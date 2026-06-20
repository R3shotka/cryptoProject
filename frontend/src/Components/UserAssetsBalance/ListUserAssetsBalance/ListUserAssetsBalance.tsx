import React, { SyntheticEvent } from 'react'
import CardUserAssetsBalance from '../CardUserAssetsBalance/CardUserAssetsBalance';

interface Props {
    UserAssetsBalanceValues: string[];
    onUserAssetsBalanceDelete: (e: SyntheticEvent) => void;
}

const ListUserAssetsBalance = ({ UserAssetsBalanceValues, onUserAssetsBalanceDelete }: Props) => {
    return (
        <section id="portfolio" className="mt-8 mb-12">
            <h2 className="mb-6 mt-3 text-3xl font-semibold text-center md:text-4xl">
                Assets Balance
            </h2>

            <div className="relative flex flex-col items-center max-w-5xl mx-auto space-y-10 px-10 mb-5 md:px-6 md:space-y-0 md:space-x-7 md:flex-row md:flex-wrap justify-center">
                {/* Перевіряємо, чи є монети в масиві */}
                {UserAssetsBalanceValues.length > 0 ? (
                    UserAssetsBalanceValues.map((UserAssetsBalanceValue) => {
                        return (
                            <CardUserAssetsBalance
                                key={UserAssetsBalanceValue}
                                UserAssetsBalanceValue={UserAssetsBalanceValue}
                                onUserAssetsBalanceDelete={onUserAssetsBalanceDelete}
                            />
                        )
                    })
                ) : (
                    // Якщо монет немає, показуємо це повідомлення
                    <h3 className="mb-3 mt-3 text-xl font-semibold text-center text-gray-500 md:text-xl w-full">
                        Your portfolio is empty.
                    </h3>
                )}
            </div>
        </section>
    )
};

export default ListUserAssetsBalance