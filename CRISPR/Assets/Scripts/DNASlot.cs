using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNASlot : MonoBehaviour {
    private Types.DNA _dnaType;
    private bool _dnaTypeSet = false;
    public delegate void DNASetHandler(Types.DNA type);
    public event DNASetHandler OnDNASet;

    public Types.DNA DNAType() {
        return _dnaType;
    }

    public void DNAType(Types.DNA type) {
        if (!_dnaTypeSet) {
            Debug.Log(type);
            _dnaType = type;
            OnDNASet(type);
        }
    }
}
