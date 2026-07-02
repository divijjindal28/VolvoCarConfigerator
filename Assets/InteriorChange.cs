/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace Oculus.Interaction.Samples
{
    public class InteriorChange : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _interiors = new List<GameObject>();

        private int _currentInteriorIndex = -1;
        private int _savedInteriorIndex = 0;

        public void NextInterior()
        {
            if (_interiors == null || _interiors.Count == 0)
            {
                Debug.LogWarning("No interiors assigned.");
                return;
            }

            _currentInteriorIndex = (_currentInteriorIndex + 1) % _interiors.Count;

            for (int i = 0; i < _interiors.Count; i++)
            {
                if (_interiors[i] != null)
                    _interiors[i].SetActive(i == _currentInteriorIndex);
            }
        }

        public void Save()
        {
            _savedInteriorIndex = _currentInteriorIndex;
        }

        public void Revert()
        {
            if (_interiors == null || _interiors.Count == 0)
                return;

            _currentInteriorIndex = _savedInteriorIndex;

            for (int i = 0; i < _interiors.Count; i++)
            {
                if (_interiors[i] != null)
                    _interiors[i].SetActive(i == _currentInteriorIndex);
            }
        }

        protected virtual void Start()
        {
            this.AssertField(_interiors, nameof(_interiors));

            if (_interiors.Count == 0)
            {
                Debug.LogWarning("No interiors assigned.");
                return;
            }

            // Find the currently active interior
            _currentInteriorIndex = 0;

            for (int i = 0; i < _interiors.Count; i++)
            {
                if (_interiors[i] != null && _interiors[i].activeSelf)
                {
                    _currentInteriorIndex = i;
                    break;
                }
            }

            // Ensure only one interior is active
            for (int i = 0; i < _interiors.Count; i++)
            {
                if (_interiors[i] != null)
                    _interiors[i].SetActive(i == _currentInteriorIndex);
            }

            _savedInteriorIndex = _currentInteriorIndex;
        }
    }
}