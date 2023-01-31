using UnityEngine;
using Unity.Netcode;
using InkeepersKeep.Core.Entities;
using InkeepersKeep.Core.Entities.Player;

namespace InkeepersKeep.Core.Network
{
    public class NetworkMovement : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _rotateSpeed;

        private IMovable _movable;
        private CameraRotation _cameraRotation;

        private int _tick = 0;
        private float _tickRate = 1f / 60f;
        private float _tickDeltaTime = 0f;

        private const int BUFFER_SIZE = 1024;
        private InputState[] _inputState = new InputState[BUFFER_SIZE];
        private TransformState[] _transformStates = new TransformState[BUFFER_SIZE];

        public NetworkVariable<TransformState> serverTransformState = new NetworkVariable<TransformState>();
        public TransformState previousTransformState;

        private void Awake()
        {
            _movable = GetComponent<IMovable>();
            _cameraRotation = GetComponentInChildren<CameraRotation>();
        }

        private void OnEnable()
        {
            serverTransformState.OnValueChanged += OnServerStateChanged;
        }

        private void OnServerStateChanged(TransformState previousValue, TransformState newValue)
        {
            previousTransformState = previousValue;
        }

        public void ProcessLocalPlayerMovement(Vector2 movementInput, Vector2 deltaMouseInput)
        {
            _tickDeltaTime += Time.deltaTime;

            if (_tickDeltaTime > _tickRate)
            {
                int bufferIndex = _tick % BUFFER_SIZE;

                if (!IsServer)
                {
                    SendInputServerRpc(_tick, movementInput, deltaMouseInput);
                    MovePlayer(movementInput);
                    RotatePlayer(deltaMouseInput);
                }
                else
                {
                    MovePlayer(movementInput);
                    RotatePlayer(deltaMouseInput);

                    TransformState state = new TransformState()
                    {
                        tick = _tick,
                        position = transform.position,
                        rotation = transform.rotation,
                        hasStartedMoving = true
                    };

                    previousTransformState = serverTransformState.Value;
                    serverTransformState.Value = state;
                }

                InputState inputState = new InputState()
                {
                    tick = _tick,
                    movementInput = movementInput,
                    deltaMouseInput = deltaMouseInput
                };

                TransformState transformState = new TransformState()
                {
                    tick = _tick,
                    position = transform.position,
                    rotation = transform.rotation,
                    hasStartedMoving = true
                };

                _inputState[bufferIndex] = inputState;
                _transformStates[bufferIndex] = transformState;

                _tickDeltaTime -= _tickRate;
                ++_tick;
            }
        }

        public void ProcessSimulatedPlayerMovement()
        {
            _tickDeltaTime += Time.deltaTime;

            if (_tickDeltaTime > _tickRate)
            {
                if (serverTransformState.Value.hasStartedMoving)
                {
                    transform.position = serverTransformState.Value.position;
                    transform.rotation = serverTransformState.Value.rotation;
                }

                _tickDeltaTime -= _tickRate;
                ++_tick;
            }
        }

        [ServerRpc]
        private void SendInputServerRpc(int tick, Vector2 movementInput, Vector2 deltaMouseInput)
        {
            MovePlayer(movementInput);
            RotatePlayer(deltaMouseInput);

            TransformState state = new TransformState()
            {
                tick = tick,
                position = transform.position,
                rotation = transform.rotation,
                hasStartedMoving = true
            };

            previousTransformState = serverTransformState.Value;
            serverTransformState.Value = state;
        }

        private void MovePlayer(Vector2 movementInput)
        {
            _movable.Move(movementInput * _speed * _tickRate);
        }

        private void RotatePlayer(Vector2 deltaMouseInput)
        {
            _cameraRotation.Rotate(deltaMouseInput * _rotateSpeed * _tickRate);
        }
    }
}
