using UnityEngine;
using System;

[CreateAssetMenu]
public class GameScenario : ScriptableObject
{
    [SerializeField] private EnemyWave[] _waves;
    [SerializeField, Range(0, 10)] private int _cycles = 1;
    [SerializeField, Range(0f, 1f)] private float _cycleSpeedUp = .5f;   

    public State Begin() => new State(this);

    [Serializable]
    public struct State
    {
        private GameScenario _scenario;

        private int _index;
        private int _cycle;
        private float _timeScale;

        private EnemyWave.State _wave;
        
        public State(GameScenario scenario)
        {
            _scenario = scenario;
            _index = 0;
            _cycle = 0;
            _timeScale = 1f;
            _wave = _scenario._waves[0].Begin();            
        }

        public (int currentWave, int wavesCount) GetWaves()
        {
            return (_index + 1, _scenario._waves.Length);
        }

        public bool Progress()
        {            
            float deltaTime = _wave.Progress(_timeScale * Time.deltaTime);

            while(deltaTime >= 0f)
            {
                if(++_index >= _scenario._waves.Length)
                {
                    if(++_cycle >= _scenario._cycles && _scenario._cycles > 0)
                    {
                        return false;
                    }
                    _index = 0;
                    _timeScale += _scenario._cycleSpeedUp;
                }
                _wave = _scenario._waves[_index].Begin();
                deltaTime = _wave.Progress(deltaTime);
            }
            return true;
        }
    }
}
