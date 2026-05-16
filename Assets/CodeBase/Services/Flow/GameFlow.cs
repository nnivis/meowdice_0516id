using System;
using System.Threading;
using System.Threading.Tasks;
using CodeBase.Domain.Match;
using UnityEngine;

namespace CodeBase.Services.Flow
{
    public class GameFlow
    {
        private readonly Match _match;
        private readonly TurnSystem _turnSystem;
        private CancellationTokenSource _cts;
        private Task _runTask;

        public GameFlow(Match match, TurnSystem turnSystem)
        {
            _match = match ?? throw new ArgumentNullException(nameof(match));
            _turnSystem = turnSystem ?? throw new ArgumentNullException(nameof(turnSystem));
        }

        public void Start()
        {
            Stop();

            _cts = new CancellationTokenSource();
            _runTask = RunAsync(_cts.Token);
        }

        public void Stop()
        {
            if (_cts == null) return;

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
            _runTask = null;
        }

        private async Task RunAsync(CancellationToken ct)
        {
            try
            {
                while (!_match.IsFinished && !ct.IsCancellationRequested)
                {
                    await _turnSystem.PlayTurnAsync(ct);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                Debug.LogError($"[GameFlow] Unhandled exception: {e}");
            }
        }
    }
    
}