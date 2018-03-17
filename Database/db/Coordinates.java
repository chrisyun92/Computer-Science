/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package db;
import java.util.ArrayList;
/**
 *
 * @author jqin7
 */
public class Coordinates {
    public ArrayList<Integer> X_indices;
    public ArrayList<Integer> Y_indices;

    public Coordinates() {
        X_indices = new ArrayList<>();
        Y_indices = new ArrayList<>();
    }

    public void put(int x, int y) {
        X_indices.add(x);
        Y_indices.add(y);
    }
    public boolean isNoValue (int x, int y) {
        for (int index = 0; index < X_indices.size(); index ++) {
            if (X_indices.get(index) == x && Y_indices.get(index) == y) {
                return true;
            }
        }
        return false;
    }

    public String toString() {
        String ret = "";
        for (int index = 0; index < X_indices.size(); index++) {
            ret += "[" + X_indices.get(index) + ", " + Y_indices.get(index) + "] ";
        }
        return ret;
    }
}