﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WasabiCli.Models;
using WasabiCli.Models.Navigation;
using WasabiCli.Models.RpcJson;
using WasabiCli.Models.WalletWasabi;

namespace WasabiCli.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private RpcServiceViewModel _rpcService;
    [ObservableProperty] private ObservableCollection<WalletViewModel>? _wallets;

    [NotifyCanExecuteChangedFor(nameof(ListCoinsCommand))]
    [NotifyCanExecuteChangedFor(nameof(ListUnspentCoinsCommand))]
    [NotifyCanExecuteChangedFor(nameof(GetWalletInfoCommand))]
    [NotifyCanExecuteChangedFor(nameof(GetNewAddressCommand))]
    [NotifyCanExecuteChangedFor(nameof(SendCommand))]
    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [NotifyCanExecuteChangedFor(nameof(GetHistoryCommand))]
    [NotifyCanExecuteChangedFor(nameof(ListKeysCommand))]
    [NotifyCanExecuteChangedFor(nameof(StartCoinJoinCommand))]
    [NotifyCanExecuteChangedFor(nameof(StopCoinJoinCommand))]
    [ObservableProperty] 
    private WalletViewModel? _selectedWallet;

    [ObservableProperty] private ObservableCollection<RpcMethodViewModel>? _rpcMethods;

    public MainWindowViewModel(INavigationService navigationService)
    {
        RpcService = new RpcServiceViewModel("http://127.0.0.1:37128");
        NavigationService = navigationService;

        Wallets = new ObservableCollection<WalletViewModel>
        {
            new () { WalletName = "Wallet 1" },
            new () { WalletName = "Wallet 2" },
            new () { WalletName = "Wallet 3" }
        };

        SelectedWallet = Wallets[0];

        RpcMethods = new ObservableCollection<RpcMethodViewModel>
        {
            new ("GetStatus", GetStatusCommand),
            new ("CreateWallet", CreateWalletCommand),
            new ("ListCoins", ListCoinsCommand),
            new ("ListUnspentCoins", ListUnspentCoinsCommand),
            new ("GetWalletInfo", GetWalletInfoCommand),
            new ("GetNewAddress", GetNewAddressCommand),
            new ("Send", SendCommand),
            new ("Build", BuildCommand),
            new ("Broadcast", BroadcastCommand),
            new ("GetHistory", GetHistoryCommand),
            new ("ListKeys", ListKeysCommand),
            new ("StartCoinJoin", StartCoinJoinCommand),
            new ("StopCoinJoin", StopCoinJoinCommand),
            new ("Stop", StopCommand)
        };
    }

    public INavigationService NavigationService { get; }

    [RelayCommand]
    private async Task GetStatus()
    {
        // {"jsonrpc":"2.0","id":"1","method":"getstatus"}
        var requestBody = new RpcMethod
        {
            Method = "getstatus"
        };
        var rpcServerUri = $"{RpcService.RpcServerPrefix}";
        var rpcResult = await RpcService.SendRpcMethod(requestBody, rpcServerUri, RpcJsonContext.Default.RpcGetStatusResult);
        if (rpcResult is RpcGetStatusResult { Result: not null } rpcGetStatusResult)
        {
            // TODO:
            NavigationService.Navigate(rpcGetStatusResult.Result);
        }
        else if (rpcResult is RpcErrorResult { Error: not null } rpcErrorResult)
        {
            // TODO:
            NavigationService.Navigate(rpcErrorResult.Error);
        }
    }

    [RelayCommand]
    private void CreateWallet()
    {
        NavigationService.Navigate(new CreateWalletViewModel(RpcService, NavigationService));
    }

    private bool CanListCoins()
    {
        return SelectedWallet?.WalletName is not null 
               && SelectedWallet?.WalletName.Length > 0;
    }

    [RelayCommand(CanExecute = nameof(CanListCoins))]
    private async Task ListCoins()
    {
        // {"jsonrpc":"2.0","id":"1","method":"listcoins"}
        var requestBody = new RpcMethod
        {
            Method = "listcoins"
        };
        var rpcServerUri = $"{RpcService.RpcServerPrefix}/{SelectedWallet?.WalletName}";
        var rpcResult = await RpcService.SendRpcMethod(requestBody, rpcServerUri, RpcJsonContext.Default.RpcListCoinsResult);
        if (rpcResult is RpcListCoinsResult { Result: not null } rpcListCoinsResult)
        {
            // TODO:
            NavigationService.Navigate(new ListCoinsInfo { Coins = rpcListCoinsResult.Result });
        }
        else if (rpcResult is RpcErrorResult { Error: not null } rpcErrorResult)
        {
            // TODO:
            NavigationService.Navigate(rpcErrorResult.Error);
        }
    }

    private bool CanListUnspentCoins()
    {
        return SelectedWallet?.WalletName is not null 
               && SelectedWallet?.WalletName.Length > 0;
    }

    [RelayCommand(CanExecute = nameof(CanListUnspentCoins))]
    private async Task ListUnspentCoins()
    {
        // {"jsonrpc":"2.0","id":"1","method":"listunspentcoins"}
        var requestBody = new RpcMethod
        {
            Method = "listunspentcoins"
        };
        var rpcServerUri = $"{RpcService.RpcServerPrefix}/{SelectedWallet?.WalletName}";
        var rpcResult = await RpcService.SendRpcMethod(requestBody, rpcServerUri, RpcJsonContext.Default.RpcListUnspentCoinsResult);
        if (rpcResult is RpcListUnspentCoinsResult { Result: not null } rpcListUnspentCoinsResult)
        {
            // TODO:
            NavigationService.Navigate(new ListUnspentCoinsInfo { Coins = rpcListUnspentCoinsResult.Result });
        }
        else if (rpcResult is RpcErrorResult { Error: not null } rpcErrorResult)
        {
            // TODO:
            NavigationService.Navigate(rpcErrorResult.Error);
        }
    }

    private bool CanGetWalletInfo()
    {
        return SelectedWallet?.WalletName is not null 
               && SelectedWallet?.WalletName.Length > 0;
    }

    [RelayCommand(CanExecute = nameof(CanGetWalletInfo))]
    private async Task GetWalletInfo()
    {
        // {"jsonrpc":"2.0","id":"1","method":"getwalletinfo"}
        var requestBody = new RpcMethod
        {
            Method = "getwalletinfo"
        };
        var rpcServerUri = $"{RpcService.RpcServerPrefix}/{SelectedWallet?.WalletName}";
        var rpcResult = await RpcService.SendRpcMethod(requestBody, rpcServerUri, RpcJsonContext.Default.RpcGetWalletInfoResult);
        if (rpcResult is RpcGetWalletInfoResult { Result: not null } rpcGetWalletInfoResult)
        {
            // TODO:
            NavigationService.Navigate(rpcGetWalletInfoResult.Result);
        }
        else if (rpcResult is RpcErrorResult { Error: not null } rpcErrorResult)
        {
            // TODO:
            NavigationService.Navigate(rpcErrorResult.Error);
        }
    }

    private bool CanGetNewAddress()
    {
        return SelectedWallet?.WalletName is not null 
               && SelectedWallet?.WalletName.Length > 0;
    }

    [RelayCommand(CanExecute = nameof(CanGetNewAddress))]
    private void GetNewAddress()
    {
        if (SelectedWallet?.WalletName is not null)
        {
            NavigationService.Navigate(new GetNewAddressViewModel(RpcService, NavigationService, SelectedWallet.WalletName));
        }
    }

    private bool CanSend()
    {
        return SelectedWallet?.WalletName is not null 
               && SelectedWallet?.WalletName.Length > 0;
    }

    [RelayCommand(CanExecute = nameof(CanSend))]
    private void Send()
    {
        if (SelectedWallet?.WalletName is not null)
        {
            NavigationService.Navigate(new SendViewModel(RpcService, NavigationService, SelectedWallet.WalletName));
        }
    }

    private bool CanBuild()
    {
        return SelectedWallet?.WalletName is not null 
               && SelectedWallet?.WalletName.Length > 0;
    }

    [RelayCommand(CanExecute = nameof(CanBuild))]
    private void Build()
    {
        if (SelectedWallet?.WalletName is not null)
        {
            NavigationService.Navigate(new BuildViewModel(RpcService, NavigationService, SelectedWallet.WalletName));
        }
    }

    [RelayCommand]
    private void Broadcast()
    {
        if (SelectedWallet?.WalletName is not null)
        {
            NavigationService.Navigate(new BroadcastViewModel(RpcService, NavigationService));
        }
    }

    private bool CanGetHistory()
    {
        return SelectedWallet?.WalletName is not null 
               && SelectedWallet?.WalletName.Length > 0;
    }

    [RelayCommand(CanExecute = nameof(CanGetHistory))]
    private async Task GetHistory()
    {
        // {"jsonrpc":"2.0","id":"1","method":"gethistory"}
        var requestBody = new RpcMethod
        {
            Method = "gethistory"
        };
        var rpcServerUri = $"{RpcService.RpcServerPrefix}/{SelectedWallet?.WalletName}";
        var rpcResult = await RpcService.SendRpcMethod(requestBody, rpcServerUri, RpcJsonContext.Default.RpcGetHistoryResult);
        if (rpcResult is RpcGetHistoryResult { Result: not null } rpcGetHistoryResult)
        {
            // TODO:
            NavigationService.Navigate(new GetHistoryInfo { Transactions = rpcGetHistoryResult.Result });
        }
        else if (rpcResult is RpcErrorResult { Error: not null } rpcErrorResult)
        {
            // TODO:
            NavigationService.Navigate(rpcErrorResult.Error);
        }
    }

    private bool CanListKeys()
    {
        return SelectedWallet?.WalletName is not null 
               && SelectedWallet?.WalletName.Length > 0;
    }

    [RelayCommand(CanExecute = nameof(CanListKeys))]
    private async Task ListKeys()
    {
        // {"jsonrpc":"2.0","id":"1","method":"listkeys"}
        var requestBody = new RpcMethod
        {
            Method = "listkeys"
        };
        var rpcServerUri = $"{RpcService.RpcServerPrefix}/{SelectedWallet?.WalletName}";
        var rpcResult = await RpcService.SendRpcMethod(requestBody, rpcServerUri, RpcJsonContext.Default.RpcListKeysResult);
        if (rpcResult is RpcListKeysResult { Result: not null } rpcListKeysResult)
        {
            // TODO:
            NavigationService.Navigate(new ListKeysInfo { Keys = rpcListKeysResult.Result });
        }
        else if (rpcResult is RpcErrorResult { Error: not null } rpcErrorResult)
        {
            // TODO:
            NavigationService.Navigate(rpcErrorResult.Error);
        }
    }

    private bool CanStartCoinJoin()
    {
        return SelectedWallet?.WalletName is not null 
               && SelectedWallet?.WalletName.Length > 0;
    }

    [RelayCommand(CanExecute = nameof(CanStartCoinJoin))]
    private void StartCoinJoin()
    {
        if (SelectedWallet?.WalletName is not null)
        {
            NavigationService.Navigate(new StartCoinJoinViewModel(RpcService, NavigationService, SelectedWallet.WalletName));
        }
    }

    private bool CanStopCoinJoin()
    {
        return SelectedWallet?.WalletName is not null 
               && SelectedWallet?.WalletName.Length > 0;
    }

    [RelayCommand(CanExecute = nameof(CanStopCoinJoin))]
    private async Task StopCoinJoin()
    {
        // {"jsonrpc":"2.0","id":"1","method":"stopcoinjoin"}
        var requestBody = new RpcMethod
        {
            Method = "stopcoinjoin"
        };
        var rpcServerUri = $"{RpcService.RpcServerPrefix}/{SelectedWallet?.WalletName}";
        var rpcResult = await RpcService.SendRpcMethod(requestBody, rpcServerUri, RpcJsonContext.Default.RpcStopCoinJoinResult);
        if (rpcResult is RpcStopCoinJoinResult)
        {
            // TODO:
            // Do nothing.
        }
        else if (rpcResult is RpcErrorResult { Error: not null } rpcErrorResult)
        {
            // TODO:
            NavigationService.Navigate(rpcErrorResult.Error);
        }
    }

    [RelayCommand]
    private async Task Stop()
    {
        // {"jsonrpc":"2.0", "method":"stop"}
        var requestBody = new RpcMethod
        {
            Method = "stop"
        };
        var rpcServerUri = $"{RpcService.RpcServerPrefix}";
        var rpcResult = await RpcService.SendRpcMethod(requestBody, rpcServerUri, RpcJsonContext.Default.RpcStopResult);
        if (rpcResult is RpcStopResult)
        {
            // TODO:
            // Do nothing.
        }
        else if (rpcResult is RpcErrorResult { Error: not null } rpcErrorResult)
        {
            // TODO:
            NavigationService.Navigate(rpcErrorResult.Error);
        }
    }
}
