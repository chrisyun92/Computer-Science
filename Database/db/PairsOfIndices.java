/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package db;

/**
 *
 * @author jqin7
 */
import java.util.ArrayList;

public class PairsOfIndices {
    private ArrayList<Integer> table1Indices;
    private ArrayList<Integer> table2Indices;

    public PairsOfIndices() {
        table1Indices = new ArrayList<>();
        table2Indices = new ArrayList<>();
    }

    public PairsOfIndices(ArrayList<Integer> t1I, ArrayList<Integer> t2I) {
        table1Indices = t1I;
        table2Indices = t2I;
    }
    public ArrayList<Integer> table1Indices() {
        return table1Indices;
    }

    public ArrayList<Integer> table2Indices() {
        return table2Indices;
    }

    public void append (PairsOfIndices pOI) {
        table1Indices.addAll(pOI.table1Indices);
        table2Indices.addAll(pOI.table2Indices);
    }

    public String toString() {
        return table1Indices.toString() + " " + table2Indices.toString();
    }

}